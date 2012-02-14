using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers
{
    public class AI1 : Player
    {
        Random rand;
        Vector[] srcs;
	    Vector[] dests;
	    int count;
        int r;

        public AI1()
            : base(new PlayerSettings(), 0)
        {
            selection = null;
            rand = new Random();
            srcs = new Vector[100];
            dests = new Vector[100];
            count = 0;
        }

	    public override void getInput()
	    {
            if (src == null)
            {
                generateActions();
                r = rand.Next(count);
                src = srcs[r];
            }
            else
            {
                System.Threading.Thread.Sleep(2000);
                dest = dests[r];
                count = 0;
            }
	    }

	    public void generateActions()
	    {
		    for(int i = 0; i < 8; i++)
			    for(int j = 0; j < 8; j++)
			    {
				    Piece tmpPc = GamePlay.board.tiles[i][j].p;
                    if (tmpPc != null && tmpPc.isHis(GamePlay.currPl))
				    {
					    Vector tmpsrc = new Vector(i, j);
					    for(int k = 0; k < tmpPc.possAttacks.Length; k++)
					    {
						    Vector tmpdest = Vector.add(tmpsrc, tmpPc.possAttacks[k]);
						    if(tmpdest.x>=0 && tmpdest.x<=7 && tmpdest.y>=0 && tmpdest.y<=7
                                && GamePlay.board.isEmpty(tmpdest)
                                && GamePlay.board.validateAttack(tmpPc, tmpsrc, tmpPc.possAttacks[k]))
							    {
								    srcs[count] = tmpsrc;
								    dests[count] = tmpdest;
								    count++;
							    }
					    }
				    }
			    }
    		
		    if(count != 0)
			    return;
    		
		    for(int i = 0; i < 8; i++)
			    for(int j = 0; j < 8; j++)
			    {
                    Piece tmpPc = GamePlay.board.tiles[i][j].p;
				    if(tmpPc != null && tmpPc.isHis(GamePlay.currPl))
				    {
					    Vector tmpsrc = new Vector(i, j);
					    for(int k = 0; k < tmpPc.possMoves.Length; k++)
					    {
						    Vector tmpdest = Vector.add(tmpsrc, tmpPc.possMoves[k]);
						    if(tmpdest.x>=0 && tmpdest.x<=7 && tmpdest.y>=0 && tmpdest.y<=7
                                && GamePlay.board.isEmpty(tmpdest)
                                && GamePlay.board.validateMove(tmpPc, tmpsrc, tmpPc.possMoves[k]))
							    {
								    srcs[count] = tmpsrc;
								    dests[count] = tmpdest;
								    count++;
							    }
					    }
				    }
			    }	
	    }
    }
}
