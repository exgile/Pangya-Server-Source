using System.Collections.Generic;
namespace PangyaAPI.Dispose.Collection
{
    public class DisposableCollection<T> where T : class, IDisposeable
    {
        private List<T> _model;

        public List<T> Model
        {
            get
            {
                //Remove pessoas Disposed
                _model?.RemoveAll(p => p != null && p.Disposed);

                return _model;
            }
            set { _model = value; }
        }

        public T this[int index]
        {
            get
            {
                return Model[index];
            }
            set
            {
                Model[index] = value;
            }
        }

        public DisposableCollection()
        {
            Model = new List<T>();
        }


        public DisposableCollection(int value)
        {
            Model = new List<T>(value);
        }


        public void Add(T pessoa)
        {
            Model.Add(pessoa);
        }
        public bool Remove(T pessoa)
        {
            return Model.Remove(pessoa);
        }

        public List<T> ToList()
        {
            return Model;
        }

        public int Count
        {
            get
            {
                return Model.Count;
            }
        }
    }
}
