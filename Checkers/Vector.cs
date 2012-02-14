using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers
{
    public class Vector
    {
        public int x;
	    public int y;

        public Vector(int a, int b)
        {
            x = a;
            y = b;
        }

	    public Vector stepBack()
	    {
		    int x1 = x>0? x-1 : x+1;
		    int y1 = y>0? y-1 : y+1;
		    return new Vector(x1, y1);
	    }

        public Vector clone()
        {
            return new Vector(x, y);
        }

        public static bool isEqual(Vector l, Vector m)
        {
            return (l.x == m.x) && (l.y == m.y);
        }

        public static Vector add(Vector l, Vector m)
        {
            return new Vector(l.x + m.x, l.y + m.y);
        }

        public static Vector sub(Vector l, Vector m)
        {
            return new Vector(l.x - m.x, l.y - m.y);
        }

        public static Vector parse(String s)
        {
            s = s.ToUpper();
            if (s.Length != 2 || s[0] < 49 || s[0] > 56
                    || s[1] < 65 || s[1] > 72)
                throw new Exception("Error parsing Coordinate,\nOnly Valid Row Number followed by Coloumn Letter is allowed.");
            return new Vector(int.Parse(s.Substring(0, 1)) - 1, s[1] - 65);
        }

        public String deParse()
        {
            int num = x + 1;
            char letter = (char)(y + 65);
            return num + "" + letter;
        }
    }
}
