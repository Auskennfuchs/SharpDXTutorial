namespace SharpDXTutorial {
    class StateMonitor<T> {
        private T initialState;
        public T State {
            get; private set;
        }

        public StateMonitor<T> Sister {
            private get; set;
        }

        public bool NeedRefresh {
            get; private set;
        }

        public StateMonitor(T initialState) {
            this.initialState = initialState;
            InitializeState();
            ResetTracking();
        }

        public void InitializeState() {
            State = initialState;
        }

        public void ResetTracking() {
            NeedRefresh = false;
        }

        public void SetState(T state) {
            State = state;

            if (Sister == null) {
                NeedRefresh = true;
                return;
            }

            NeedRefresh = !SameAsSister();
        }

        public bool SameAsSister() {
            if(State==null && Sister.State!=null) {
                return false;
            }
            return State.Equals(Sister.State);
        }

        public void ApplyValue(StateMonitor<T> source) {
            State = source.State;
            NeedRefresh = source.NeedRefresh;
        }
    }
}
