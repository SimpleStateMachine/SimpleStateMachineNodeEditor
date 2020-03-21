namespace SimpleStateMachineNodeEditor.Helpers.Commands
{
    public class ValidateObjectProperty<TObject, TProperty> where TObject : class where TProperty : class
    {
        public ValidateObjectProperty(TObject obj, TProperty property)
        {
            Obj = obj;
            Property = property;
        }
        /// <summary>
        /// Объект, для которого будем валидировать значенин
        /// </summary>
        public TObject Obj { get; set; }

        /// <summary>
        /// Новое значение, которое будем валидировать
        /// </summary>
        public TProperty Property { get; set; }

    }
}
