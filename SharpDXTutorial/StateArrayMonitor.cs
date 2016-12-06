using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDXTutorial {
    class StateArrayMonitor<T> {
        private T initialState;
        public T[] States {
            get; private set;
        }

        private int numStates;

        public StateArrayMonitor<T> Sister {
            private get; set;
        }

        public int StartSlot {
            get; private set;
        }

        public int EndSlot {
            get; private set;
        }

        public bool NeedRefresh {
            get; private set;
        }

        public StateArrayMonitor(T initialState, int numStates) {
            this.initialState = initialState;
            this.numStates = numStates;
            States = new T[numStates];
            InitializeStates();
            ResetTracking();
        }

        public void InitializeStates() {
            for(int i=0; i<numStates;i++) {
                States[i] = initialState;
            }
        }

        public void ResetTracking() {
            NeedRefresh = false;
            StartSlot = EndSlot = 0;
        }

        public void SetState(int slot,T state) {
            States[slot] = state;

            if(Sister==null) {
                NeedRefresh = true;
                StartSlot = slot;
                EndSlot = numStates - 1;

                return;
            }

            var sameSister = SameAsSister(slot);
            if(!NeedRefresh && !sameSister) {
                NeedRefresh = true;
                StartSlot = slot;
                EndSlot = slot;

                return;
            }

            if(NeedRefresh) {
                if(slot < StartSlot) {
                    if(!sameSister) {
                        StartSlot = slot;
                    }
                } else if(slot == StartSlot) {
                    if(sameSister) {
                        SearchFromBelow();
                    }
                } else if (slot == EndSlot) {
                    if(sameSister) {
                        SearchFromAbove();
                    }
                } else if (slot > EndSlot) {
                    if(!sameSister) {
                        EndSlot = slot;
                    }
                }
            }
        }

        public T GetState(int slot) {
            return States[slot];
        }

        public bool SameAsSister(int slot) {
            return States[slot].Equals(Sister.States[slot]);
        }

        private void SearchFromBelow() {
            for (; StartSlot < EndSlot; StartSlot++) {
                if(!SameAsSister(StartSlot)) {
                    break;
                }
            }
            if(StartSlot==EndSlot && SameAsSister(StartSlot)) {
                ResetTracking();
            }
        }

        private void SearchFromAbove() {
            for (; EndSlot > StartSlot; EndSlot--) {
                if (!SameAsSister(EndSlot)) {
                    break;
                }
            }
            if (StartSlot == EndSlot && SameAsSister(EndSlot)) {
                ResetTracking();
            }
        }

        public T[] GetChangedRange() {
            if(NeedRefresh) {
                var result = new T[EndSlot - StartSlot + 1];
                for(int i = StartSlot, j=0; i <= EndSlot; i++, j++) {
                    result[j] = States[i];
                }
                return result;
            }

            return null;
        }

        public T[] GetRange(int minSlot, int maxSlot) {
            var result = new T[EndSlot - StartSlot + 1];
            for (int i = minSlot, j = 0; i <= maxSlot; i++, j++) {
                result[j] = States[i];
            }
            return result;
        }

        public void ApplyValues(StateArrayMonitor<T> source) {
            for (int i = 0; i < numStates; i++) {
                States[i] = source.States[i];
            }
            NeedRefresh = source.NeedRefresh;
            StartSlot = source.StartSlot;
            EndSlot = source.EndSlot;
        }
    }
}
