using System;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Models.Interfaces;

namespace Tweetinvi.Core.Web
{
    public interface ITwitterResultFactory
    {
        ITwitterResult<DTO> Create<DTO>(ITwitterRequest request, ITwitterResponse response) where DTO : class;

        ITwitterResult<DTO, Model> Create<DTO, Model>(
            ITwitterRequest request, 
            ITwitterResponse response,
            Func<DTO, Model> convert) 
            where DTO : class 
            where  Model : class;

        ITwitterResult<DTO, Model> Create<DTO, Model>(ITwitterResult<DTO> result, Func<DTO, Model> convert)
            where DTO : class
            where Model : class;
    }

    public class TwitterResultFactory : ITwitterResultFactory
    {
        private readonly IJsonObjectConverter _jsonObjectConverter;

        public TwitterResultFactory(IJsonObjectConverter jsonObjectConverter)
        {
            _jsonObjectConverter = jsonObjectConverter;
        }

        public ITwitterResult<T> Create<T>(ITwitterRequest request, ITwitterResponse response) where T : class
        {
            return new TwitterResult<T>(_jsonObjectConverter)
            {
                Response = response,
                Request = request
            };
        }

        public ITwitterResult<DTO, Model> Create<DTO, Model>(ITwitterRequest request, ITwitterResponse response, Func<DTO, Model> convert) where DTO : class where Model : class
        {
            return new TwitterResult<DTO, Model>(_jsonObjectConverter, convert)
            {
                Response = response,
                Request = request
            };
        }

        public ITwitterResult<DTO, Model> Create<DTO, Model>(ITwitterResult<DTO> result, Func<DTO, Model> convert) where DTO : class where Model : class
        {
            return Create(result.Request, result.Response, convert);
        }
    }

    public interface ITwitterResult<DTO>
    {
        // ReSharper disable once UnusedMemberInSuper.Global
        DTO DataTransferObject { get; set; }
        ITwitterResponse Response { get; set; }
        ITwitterRequest Request { get; set; }
    }

    public interface ITwitterResult<DTO, Model> : ITwitterResult<DTO>
        where DTO : class
        where Model : class
    {
        Model Result { get; }
    }

    public class TwitterResult<DTO> : ITwitterResult<DTO> where DTO : class
    {
        private readonly IJsonObjectConverter _jsonObjectConverter;

        private bool _initialized;
        private DTO _result;

        public TwitterResult(IJsonObjectConverter jsonObjectConverter)
        {
            _jsonObjectConverter = jsonObjectConverter;
        }

        public DTO DataTransferObject
        {
            get
            {
                if (!_initialized)
                {
                    _initialized = true;

                    var json = Response.Text;
                    var converters = Request.Config.Converters;

                    _result = _jsonObjectConverter.DeserializeObject<DTO>(json, converters);
                }

                return _result;
            }
            set
            {
                _initialized = true;
                _result = value;
            }
        }
        public ITwitterResponse Response { get; set; }
        public ITwitterRequest Request { get; set; }
    }

    public class TwitterResult<DTO, Model> : TwitterResult<DTO>, ITwitterResult<DTO, Model>
        where DTO : class 
        where Model : class
    {
        private readonly Func<DTO, Model> _convert;
        private Model _result;

        public TwitterResult(
            IJsonObjectConverter jsonObjectConverter,
            Func<DTO, Model> convert) 
            : base(jsonObjectConverter)
        {
            _convert = convert;
        }

        public Model Result
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
