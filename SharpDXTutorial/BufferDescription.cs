using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;

namespace SharpDXTutorial.Resources {
    class VertexElementDescription {
        private List<InputElement> description = new List<InputElement>();

        public int HashCode {
            get; private set;
        }

        public InputElement[] Elements {
            get {
                return description.ToArray();
            }
        }

        public VertexElementDescription() {
        }

        public VertexElementDescription(List<InputElement> elements) {
            description.AddRange(elements);
            HashCode = GetHashCode();
        }

        public void AddDescription(InputElement element) {
            description.Add(element);
            HashCode = GetHashCode();
        }

        private new int GetHashCode() {
            int hashCode = description.Count;
            foreach (var d in description) {
                hashCode = (hashCode * 397) ^ d.GetHashCode();
            }
            return hashCode;
        }
    }
}
