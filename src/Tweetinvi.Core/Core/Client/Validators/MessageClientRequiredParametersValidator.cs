using System;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IMessagesClientRequiredParametersValidator : IMessagesClientParametersValidator
    {
    }

    public class MessagesClientRequiredParametersValidator : IMessagesClientRequiredParametersValidator
    {
        public void Validate(IPublishMessageParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (string.IsNullOrEmpty(parameters.Text))
            {
                throw new ArgumentNullException($"{nameof(parameters.Text)}");
            }
        }

        public void Validate(IDeleteMessageParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
        }

        public void Validate(IGetMessageParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
        }

        public void Validate(IGetMessagesParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
        }
    }
}