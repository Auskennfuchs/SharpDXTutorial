namespace SharpDXTutorial.Resources {
    public enum ConstantBufferParameterType {
        MATRIX,
        VECTOR3,
        VECTOR4,
        NUM_ELEM
    }

    public interface IConstantBufferParameter {
        int GetSize();
        byte[] GetBytes();
        void SetValue(object obj);
        object GetValue();
        ConstantBufferParameterType GetType();
    }

    class ConstantBufferParameter {
        public string Name {
            get;
        }

        public int Offset {
            get;
        }

        public IConstantBufferParameter Param {
            get;
        }

        public int Size {
            get {
                return Param.GetSize();
            }
        }

        public object Value {
            get {
                return Param.GetValue();
            }
            set {
                Param.SetValue(value);
            }
        }

        public ConstantBufferParameter(string name, int offset, IConstantBufferParameter param) {
            Name = name;
            Offset = offset;
            Param = param;
        }
    }
}
