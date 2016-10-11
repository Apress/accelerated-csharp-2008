sealed public class Point
{
    // other methods removed for clarity
    
    public override bool Equals( object other ) {
        bool result = false;
        Point that = other as Point;
        if( that != null ) {
            result = (this.coordinates == that.coordinates);
        }

        return result;
    }

    public override int GetHashCode() {
        return precomputedHash;
    }

    public static bool operator ==( Point pt1, Point pt2 ) {
        if( pt1.GetHashCode() != pt2.GetHashCode() ) {
            return false;
        } else {
            return Object.Equals( pt1, pt2 );
        }
    }

    public static bool operator !=( Point pt1, Point pt2 ) {
        if( pt1.GetHashCode() != pt2.GetHashCode() ) {
            return true;
        } else {
            return !Object.Equals( pt1, pt2 );
        }
    }

    private float[] coordinates;
    private int precomputedHash;
}
