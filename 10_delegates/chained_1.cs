using System;

public delegate double ProcessResults( double x,
                                       double y );

public class Processor
{
    public Processor( double factor ) {
        this.factor = factor;
    }

    public double Compute( double x, double y ) {
        double result = (x+y)*factor;
        Console.WriteLine( "InstanceResults: {0}", result );
        return result;
    }

    public static double StaticCompute( double x,
                                        double y ) {
        double result = (x+y)*0.5;
        Console.WriteLine( "StaticResult: {0}", result );
        return result;
    }

    private double factor;
}

public class EntryPoint
{
    static void Main() {
        Processor proc1 = new Processor( 0.75 );
        Processor proc2 = new Processor( 0.83 );

        ProcessResults[] delegates = new ProcessResults[] {
            new ProcessResults( proc1.Compute ),
            new ProcessResults( proc2.Compute ),
            new ProcessResults( Processor.StaticCompute )
        };

        ProcessResults chained = (ProcessResults) Delegate.Combine( delegates );

        double combined = chained( 4, 5 );
        
        Console.WriteLine( "Output: {0}", combined );
    }
}
