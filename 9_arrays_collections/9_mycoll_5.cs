using System;
using System.Threading;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public struct ImmutableBool
{
    public ImmutableBool( bool b ) {
        this.b = b;
    }

    public bool Value {
        get { return b; }
    }

    private bool b;
}

public class MyColl<T> : IEnumerable<T>
{
    public MyColl( T[] items ) {
        this.items = items;
    }

    public IEnumerator<T> GetEnumerator( 
                           ImmutableBool synchronized ) {
        if( synchronized.Value ) {
            Monitor.Enter( items.SyncRoot );
        }
        try {
            foreach( T item in items ) {
                yield return item;
            }
        }
        finally {
            if( synchronized.Value ) {
                Monitor.Exit( items.SyncRoot );
            }
        }
    }

    public IEnumerator<T> GetEnumerator() {
        return GetEnumerator( new ImmutableBool(false) );
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    private T[] items;
}

public class EntryPoint
{
    static void Main() {
        MyColl<int> integers =
            new MyColl<int>( new int[] {1, 2, 3, 4} );

        IEnumerator<int> enumerator =
            integers.GetEnumerator( new ImmutableBool(true) );

        // Get reference to synchronized field.
        object field = enumerator.GetType().
            GetField("synchronized").GetValue( enumerator );

        while( enumerator.MoveNext() ) {
            Console.WriteLine( enumerator.Current );

            // Throws an exception.
            field.GetType().GetProperty("Value").
                SetValue( field, false, null );
        }
    }
}
