using System;
using System.Runtime.Serialization;

[Serializable()]
public class EmployeeVerificationException : Exception
{
    public enum Cause {
        InvalidSSN,
        InvalidBirthDate
    }

    public EmployeeVerificationException( Cause reason )
        :base() {
        this.reason = reason;
    }

    public EmployeeVerificationException( Cause reason,
                                          String msg )
        :base( msg ) {
        this.reason = reason;
    }

    public EmployeeVerificationException( Cause reason,
                                          String msg,
                                          Exception inner )
        :base( msg, inner ) {
        this.reason = reason;
    }

    protected EmployeeVerificationException(
                   SerializationInfo info,
                   StreamingContext  context )
                :base( info, context ) { }

    private Cause reason;
    public Cause Reason {
        get {
            return reason;
        }
    }
}
