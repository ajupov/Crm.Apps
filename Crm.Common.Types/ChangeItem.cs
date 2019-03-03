namespace Crm.Common.Types
{
    public class ChangeItem
    {
        public ChangeType ChangeType { get; set; }

        public string FieldPath { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
}