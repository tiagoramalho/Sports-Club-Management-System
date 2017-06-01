using System;
using System.Collections.ObjectModel;

namespace CluSys.lib
{
    internal class MajorProblem
    {
        public int Id { get; set; } = -1;
        public string Description { get; set; }
        public int EvalId { get; set; }
        public int SessionId { get; set; }

        public ObservableCollection<MajorProblem> Container;
        public int CountId => Container?.IndexOf(this) + 1 ?? Id;

        public MajorProblem(ObservableCollection<MajorProblem> container = null) { Container = container; }

        private bool Equals(MajorProblem other)
        {
            if (Id != -1 && other.Id != -1)
                return Id == other.Id;
            return string.Equals(Description, other.Description, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((MajorProblem) obj);
        }

        public override int GetHashCode()
        {
            if (Id != -1)
                return Id.GetHashCode();
            return Description != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Description) : 0;
        }
    }
}
