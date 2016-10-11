using System;
using System.Reflection;

public delegate int Operation( int x, int y );

public class Bind2nd<Arg1Type, Arg2Type, ReturnType>
{
    public delegate ReturnType UnboundDelegate<UBArg1Type, UBArg2Type>( UBArg1Type arg1, UBArg2Type arg2 );
    public delegate ReturnType BoundDelegate<BArg1Type>( BArg1Type x );

    public Bind2nd( Delegate del, Arg2Type arg2 ) {
        // Get the types from the delegate.
        object target = del.Target;
        MethodInfo targetMethod = del.Method;
        Type targetType = targetMethod.ReflectedType;
        
        if( target == null ) {
            // Static method
            this.del = (UnboundDelegate<Arg1Type, Arg2Type>) Delegate.CreateDelegate(
                            typeof(UnboundDelegate<Arg1Type, Arg2Type>),
                            targetType,
                            targetMethod.Name );

        } else {
            // Instance method
            this.del = (UnboundDelegate<Arg1Type, Arg2Type>) Delegate.CreateDelegate(
                            typeof(UnboundDelegate<Arg1Type, Arg2Type>),
                            target,
                            targetMethod.Name );
        }
        this.arg2 = arg2;
    }

    public BoundDelegate<Arg1Type> Binder {
        get {
            return delegate( Arg1Type arg1 ) {
                return del( arg1, arg2 );
            };
        }
    }

    private UnboundDelegate<Arg1Type, Arg2Type> del;
    private Arg2Type arg2;
}

public class EntryPoint
{
    static int Add( int x, int y ) {
        return x + y;
    }

    static void Main() {
        Bind2nd<int,int,int> binder = new Bind2nd<int,int,int>(
                new Operation(EntryPoint.Add),
                4 );

        Console.WriteLine( binder.Binder(2) );
    }
}
