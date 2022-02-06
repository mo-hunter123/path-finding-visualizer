using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PathFindingOff
{
    enum VerticeType
    {
        EDGE_LEFT_TOP, 
        EDGE_LEFT, 
        EDGE_LEFT_BOTTOM, 
        EDGE_BOTTOM, 
        EDGE_RIGHT_BOTTOM, 
        EDGE_RIGHT, 
        EDGE_RIGHT_TOP,
        EDGE_TOP, 
        NORMAL
    }
    enum States
    {
        OBSTACLE,
        FREE,
        STARTPOINT,
        ENDPOINT
    };

    class CustomizedRectangle
    {
        public Rectangle rectangle;
        public States etat;
        public VerticeType type; 

        // parameters for A* star algorithm 
        double f, g, h;


        public CustomizedRectangle(Rectangle rec)
        {
            this.rectangle = rec;
            this.etat = States.FREE;
            this.type = VerticeType.NORMAL; 
        }


    }
}
