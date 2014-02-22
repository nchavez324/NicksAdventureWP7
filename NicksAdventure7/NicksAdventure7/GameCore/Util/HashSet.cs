using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NicksAdventure7.GameCore.Util{
    public class HashSet<T>{
        Dictionary<int, T> set;
        public int Count;

        public HashSet(){
            Count = 0;
            set = new Dictionary<int, T>();
        }
        public void Add(T item){
            set[Count] = item;
            Count++;
        }
        public void Remove(T item){
            int j = -1;
            foreach (int i in set.Keys){
                if (item.Equals(set[i])){
                    j = i;
                }
            }
            if(j >= 0 && j < set.Count()){
                set.Remove(j);
                Count--;
                for (int k = j+1; k < Count+1; k++){
                    T temp = set[k];
                    set[k - 1] = temp;
                }
            }
        }
        public T ElementAt(int i){
            //err on remove correct indices of remaining items
            return set[i];
        }
    }
}
