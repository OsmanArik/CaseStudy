using Castle.DynamicProxy;
using FluentValidation;
using Shared.Core.Constants;
using Shared.Core.Middleware.Validation;
using Shared.Core.Utilies.Interceptors;

namespace Shared.Core.Aspects.Autofact.Validation
{
    public class ValidationAspect : MethodInterception
    {
        #region Variable

        private Type _validatorType;

        #endregion

        #region Constructor

        public ValidationAspect() { }

        #endregion

        #region Methods

        #region Public Methods

        public ValidationAspect(Type validatorType)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new Exception(AspectMessages.WrongValidationType);
            }

            _validatorType = validatorType;
        }

        #endregion

        #region Protected Methods

        protected override void OnBefore(IInvocation invocation)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);

            var entityType = _validatorType.BaseType.GetGenericArguments()[0];

            var entities = invocation.Arguments.Where(t => t.GetType() == entityType);

            foreach (var entity in entities)
                ValidationTool.Validate(validator, entity);
        }

        #endregion

        #endregion

    }
}