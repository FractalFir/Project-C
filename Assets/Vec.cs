using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class VecEnumerator<T> : IEnumerator<T> {
    int pos = -1;
    Vec<T> inner;
    public T Current{
        get => inner[pos];
    } 
    object IEnumerator.Current{
        get => inner[pos];
    } 
    public void Reset() => pos = -1;
    public bool MoveNext(){
        pos += 1;
        return pos < inner.Length;
    }
    public void Dispose(){}
    public VecEnumerator(Vec<T> inner){
        this.inner = inner;
    }
}
public class Vec<T> : IEnumerable
{
    const int DEFAULT_CAPACITY = 0x100;
    T[] objects;
    int length;
    public int capacity{
        get => objects.Length;
    }
    public int Length{
        get => length;
    }
    public Vec(){
        this.objects = new T[DEFAULT_CAPACITY];
        this.length = 0;
    }
    public Vec(int capacity){
        this.objects = new T[capacity];
        this.length = 0;
    }
    static int GetNewCap(int prevCap) => prevCap*2;
    public void Push(T value){
        if(length>= capacity){
            T[] newArray = new T[GetNewCap(capacity)];
            System.Array.Copy(objects,newArray,capacity);
            objects = newArray;
        }
        objects[length] = value;
        length += 1;
    }
    public T[] GetInnerArray() => objects;
    public IEnumerator GetEnumerator() => new VecEnumerator<T>(this);
    public T this[int index]{
        get{
            if(index > length)throw new System.IndexOutOfRangeException();
            return objects[index];
        }
        set {
            if(index > length)throw new System.IndexOutOfRangeException();
            objects[index] = value;
        }
    }
}
