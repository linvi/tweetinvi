using System;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Web
{
    public interface ITwitterResultFactory
    {
        ITwitterResult Create(ITwitterRequest request, ITwitterResponse response);
        ITwitterResult<TDTO> Create<TDTO>(ITwitterRequest request, ITwitterResponse response) where TDTO : class;

        ITwitterResult<TDTO, TModel> Create<TDTO, TModel>(
            ITwitterRequest request,
            ITwitterResponse response,
            Func<TDTO, TModel> convert)
            where TDTO : class
            where TModel : class;

        ITwitterResult<TDTO, TModel> Create<TDTO, TModel>(ITwitterResult<TDTO> result, Func<TDTO, TModel> convert)
            where TDTO : class
            where TModel : class;
    }

    public class TwitterResultFactory : ITwitterResultFactory
    {
        private readonly IJsonObjectConverter _jsonObjectConverter;

        public TwitterResultFactory(IJsonObjectConverter jsonObjectConverter)
        {
            _jsonObjectConverter = jsonObjectConverter;
        }

        public ITwitterResult Create(ITwitterRequest request, ITwitterResponse response)
        {
            return new TwitterResult
            {
                Response = response,
                Request = request
            };
        }

        public ITwitterResult<T> Create<T>(ITwitterRequest request, ITwitterResponse response) where T : class
        {
            return new TwitterResult<T>(_jsonObjectConverter)
            {
                Response = response,
                Request = request
            };
        }

        public ITwitterResult<TDTO, TModel> Create<TDTO, TModel>(ITwitterRequest request, ITwitterResponse response, Func<TDTO, TModel> convert)
            where TDTO : class where TModel : class
        {
            return new TwitterResult<TDTO, TModel>(_jsonObjectConverter, convert)
            {
                Response = response,
                Request = request
            };
        }

        public ITwitterResult<TDTO, TModel> Create<TDTO, TModel>(ITwitterResult<TDTO> result, Func<TDTO, TModel> convert) where TDTO : class where TModel : class
        {
            return Create(result.Request, result.Response, convert);
        }
    }

    public interface ITwitterResult
    {
        ITwitterResponse Response { get; set; }
        ITwitterRequest Request { get; set; }
        string RawResult { get; }
    }

    public interface ITwitterResult<out TDTO> : ITwitterResult
    {
        TDTO DataTransferObject { get; }
    }

    public interface ITwitterResult<out TDTO, out TModel> : ITwitterResult<TDTO>
    {
        TModel Result { get; }
    }

    public class TwitterResult : ITwitterResult
    {
        public ITwitterResponse Response { get; set; }
        public ITwitterRequest Request { get; set; }
        public string RawResult => Response?.Text;
    }

    public class TwitterResult<TDTO> : TwitterResult, ITwitterResult<TDTO>
    {
        private readonly IJsonObjectConverter _jsonObjectConverter;

        private bool _initialized;
        private TDTO _result;

        public TwitterResult()
        {
        }

        public TwitterResult(IJsonObjectConverter jsonObjectConverter)
        {
            _jsonObjectConverter = jsonObjectConverter;
        }

        public TDTO DataTransferObject
        {
            get
            {
                if (!_initialized)
                {
                    _initialized = true;

                    var json = Response?.Text;
                    var converters = Request.ExecutionContext.Converters;

                    _result = _jsonObjectConverter.DeserializeObject<TDTO>(json, converters);
                }

                return _result;
            }
            set
            {
                _initialized = true;
                _result = value;
            }
        }
    }

    public class TwitterResult<TDTO, TModel> : TwitterResult<TDTO>, ITwitterResult<TDTO, TModel>
        where TDTO : class
        where TModel : class
    {
        private readonly Func<TDTO, TModel> _convert;
        private TModel _result;

        public TwitterResult(IJsonObjectConverter jsonObjectConverter, Func<TDTO, TModel> convert)
            : base(jsonObjectConverter)
        {
            _convert = convert;
        }

        public TModel Result
        {
            get
            {
                var dto = DataTransferObject;

                if (dto == null)
                {
                    return null;
                }

                if (_result == null)
                {
                    _result = _convert(dto);
                }

                return _result;
            }
        }
    }
}