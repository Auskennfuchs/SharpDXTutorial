﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDXTutorial.Resources;

namespace SharpDXTutorial {
    class ParameterManager {
        Dictionary<string, IConstantBufferParameter> parameters = new Dictionary<string, IConstantBufferParameter>();

        private static string WORLDMATRIX = "worldMatrix";
        private static string VIEWMATRIX = "viewMatrix";
        private static string PROJMATRIX = "projMatrix";
        private static string WORLDVIEWPROJMATRIX = "worldViewProjMatrix";

        public ParameterManager() {
            SetParameter(WORLDMATRIX, Matrix.Identity);
            SetParameter(VIEWMATRIX, Matrix.Identity);
            SetParameter(PROJMATRIX, Matrix.Identity);
            SetParameter(WORLDVIEWPROJMATRIX, Matrix.Identity);
        }

        public void SetParameter(string name, Matrix mat) {
            if (!SetParam(name, mat, ConstantBufferParameterType.MATRIX)) {
                parameters.Add(name, new MatrixParameter(mat));
            }
        }

        public IConstantBufferParameter GetParameter(string name) {
            if (parameters.ContainsKey(name)) {
                return parameters[name];
            }
            return null;
        }

        public Matrix GetMatrixParameter(string name) {
            return (Matrix)GetParam(name, ConstantBufferParameterType.MATRIX);
        }

        public void SetParameter(string name, Vector3 vec) {
            if (!SetParam(name, vec, ConstantBufferParameterType.VECTOR3)) {
                throw new NotImplementedException();
            }
        }
        public Vector3 GetVector3Parameter(string name) {
            return (Vector3)GetParam(name, ConstantBufferParameterType.VECTOR3);
        }

        public void SetParameter(string name, Vector4 vec) {
            if (!SetParam(name, vec, ConstantBufferParameterType.VECTOR4)) {
                throw new NotImplementedException();
            }
        }
        public Vector4 GetVector4Parameter(string name) {
            return (Vector4)GetParam(name, ConstantBufferParameterType.VECTOR4);
        }

        public void SetWorldMatrix(Matrix world) {
            SetParameter(WORLDMATRIX, world);
            UpdateMatrices();
        }
        public void SetViewMatrix(Matrix view) {
            SetParameter(VIEWMATRIX, view);
            UpdateMatrices();
        }
        public void SetProjectionMatrix(Matrix projection) {
            SetParameter(PROJMATRIX, projection);
            UpdateMatrices();
        }

        public bool HasParameter(string name) {
            return parameters.ContainsKey(name);
        }

        private bool SetParam(string name, object obj, ConstantBufferParameterType type) {
            if (parameters.ContainsKey(name)) {
                var param = parameters[name];
                if (param.GetType() != type) {
                    throw new NotSupportedException("Wrong Parametertype expected " + type + " but was " + param.GetType());
                }
                param.SetValue(obj);
                return true;
            }
            return false;
        }

        private object GetParam(string name, ConstantBufferParameterType type) {
            if (parameters.ContainsKey(name)) {
                var param = parameters[name];
                if (param.GetType() != type) {
                    throw new NotSupportedException("Wrong Parametertype expected " + type + " but was " + param.GetType());
                }
                return param.GetValue();
            }
            return null;
        }

        private void UpdateMatrices() {
            Matrix world = (Matrix)GetParam(WORLDMATRIX, ConstantBufferParameterType.MATRIX);
            Matrix view = (Matrix)GetParam(VIEWMATRIX, ConstantBufferParameterType.MATRIX);
            Matrix proj = (Matrix)GetParam(PROJMATRIX, ConstantBufferParameterType.MATRIX);
            SetParameter(WORLDVIEWPROJMATRIX, world * view * proj);
        }
    }
}
