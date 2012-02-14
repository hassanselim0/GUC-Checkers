using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Checkers
{
    public class Board
    {
        public static Model model;
        public static Matrix[] aTrans;

        public Tile[][] tiles;
        public Vector attackAgain;
        public bool fruled = false;

        public Board()
        {
            tiles = new Tile[8][];

            for (int i = 0; i < 8; i++)
            {
                tiles[i] = new Tile[8];
                for (int j = 0; j < 8; j++)
                {
                    if (((j == 0 || j == 2) && i % 2 != 0) || (j == 1 && i % 2 == 0))
                        tiles[i][j] = new Tile(new BlackPc(GamePlay.currPl.next, new Vector3(i * 5, 0, j * 5)));
                    else if (((j == 5 || j == 7) && i % 2 == 0) || (j == 6 && i % 2 != 0))
                        tiles[i][j] = new Tile(new WhitePc(GamePlay.currPl, new Vector3(i * 5, 0, j * 5)));

                    else if ((j == 3 && i % 2 == 0) || (j == 4 && i % 2 != 0))
                        tiles[i][j] = new Tile(null);
                    else
                        tiles[i][j] = new Tile(null);
                }
            }
        }

        public static void setModel(Model m)
        {
            model = m;
            BasicEffect e = (BasicEffect)m.Meshes[0].Effects[0];
            e.EnableDefaultLighting();
            e.PreferPerPixelLighting = true;
            e.SpecularPower = 200;
            e.DirectionalLight0.DiffuseColor = Vector3.One;
            e.DirectionalLight0.SpecularColor = Vector3.One;
            e.DirectionalLight1.Enabled = false;
            e.DirectionalLight2.Enabled = false;
            
            aTrans = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(aTrans);
        }

        public Piece getPc(Vector v)
        {
            return tiles[v.x][v.y].p;
        }

        public bool isEmpty(Vector v)
        {
            return tiles[v.x][v.y].p == null;
        }

        public void applyAction(Vector src, Vector dest)
	    {
		    if(isPossAttack(src, dest))
		    {
			    attackPc(src, dest);

                if (isMultipleAttack(dest))
                    attackAgain = dest;
                else
                    attackAgain = null;
		    }
            else if (isPossMove(src, dest))
            {
                fRule();
                movePc(src, dest);
            }
            else
            {
                GamePlay.currPl.dest = null;
                throw new GameplayException("Invalid Action !");
            }
	    }

        public void movePc(Vector src, Vector dest)
        {
            tiles[dest.x][dest.y].p = tiles[src.x][src.y].p;
            tiles[src.x][src.y].p = null;

            if (tiles[dest.x][dest.y].p != null)
                tiles[dest.x][dest.y].p.setTarget(dest);

            makeKing(dest);
        }

        public void attackPc(Vector src, Vector dest)
        {
            Vector enemyPos = Vector.add(findEnemy(Vector.sub(dest, src), src), src);
            tiles[enemyPos.x][enemyPos.y].killPc = true;

            movePc(src, dest);

            makeKing(dest);
        }

        public bool isMultipleAttack(Vector src)
        {
            Piece srcPc = getPc(src);

            for (int i = 0; i < srcPc.possAttacks.Length; i++)
            {
                Vector dest = Vector.add(src, srcPc.possAttacks[i]);
                if (dest.x >= 0 && dest.x <= 7 && dest.y >= 0 && dest.y <= 7 && isEmpty(dest)
                    && validateAttack(srcPc, src, srcPc.possAttacks[i]))
                    return true;
            }

            return false;
        }

        public void validateSrcPc(Vector v)
	    {
            Piece temp = tiles[v.x][v.y].p;
		    if(temp == null)
                throw new GameplayException("The selected Tile is Empty !");
		    if(!temp.isHis(GamePlay.currPl))
                throw new GameplayException("It's not your Piece !");
	    }
    	
	    public void validateDestPc(Vector v)
	    {
            if (tiles[v.x][v.y].p != null)
                throw new GameplayException("The destination Tile is Occupied !");
	    }
    	
	    public bool isPossMove(Vector src, Vector dest)
	    {
            Piece srcPc = tiles[src.x][src.y].p;
		    for(int i = 0; i < srcPc.possMoves.Length; i++)
			    if(Vector.isEqual(Vector.add(src, srcPc.possMoves[i]), dest))
				    return validateMove(srcPc, src, srcPc.possMoves[i]);
		    return false;
	    }
    	
	    public bool validateMove(Piece srcPc, Vector src, Vector possMove)
	    {
            return isKingClear(possMove.stepBack(), src);
	    }
    	
	    public bool isPossAttack(Vector src, Vector dest)
	    {
		    //do checking for possible attacks here ...
            Piece srcPc = tiles[src.x][src.y].p;
		    for(int i = 0; i < srcPc.possAttacks.Length; i++)
			    if(Vector.isEqual(Vector.add(src, srcPc.possAttacks[i]), dest))
				    return validateAttack(srcPc, src, srcPc.possAttacks[i]);
		    return false;
	    }
    	
	    public bool validateAttack(Piece srcPc, Vector src, Vector possAttack)
	    {
            Vector tmp = findEnemy(possAttack, src); //Finds the enemy
		    if(tmp != null) //If the enemy exists.
			    return isKingClear(tmp.stepBack(), src); //Check if the path between enemy and king is clear.
		    return false; //Else return false cause there is no enemy !
	    }

        public Vector findEnemy(Vector curr, Vector src)
        {
            if (curr.x == 0 && curr.y == 0) //Reached the attacking King ?
                return null;

            Vector add = Vector.add(curr, src); //it is the actual current position
            if (!isEmpty(add)) //if you found a piece
                if (!getPc(add).isHis(GamePlay.currPl)) //Found the enemy ?
                    return curr;
                else
                    return null; //Your Piece is blocking the way between the dest and enemy (if it existed)

            return findEnemy(curr.stepBack(), src);
        }

        public bool isKingClear(Vector curr, Vector src) //Checks if the path for the King is Clear.
        {
            if (curr.x == 0 && curr.y == 0) //Reached the attacking King ?
                return true;

            Vector add = Vector.add(curr, src);
            if (!isEmpty(add)) //Is something blocking the way ?
                return false;

            return true && isKingClear(curr.stepBack(), src);
        }

        public void makeKing(Vector v) //Check for promotion to King and apply it
        {
            if (!isEmpty(v))
                tiles[v.x][v.y].p = getPc(v).promote(v.y);
        }

        public void fRule() //Rule number (f) in the .pdf :)
	    {
		    Tile[] delete = new Tile[12]; //12 penalties at maximum.
		    int count = 0;
    		
		    for(int i = 0; i < 8; i++)
			    for(int j = 0; j < 8; j++)
			    {
				    Piece tmpPc = tiles[i][j].p;
				    if(tmpPc != null && tmpPc.isHis(GamePlay.currPl))
				    {
					    Vector src = new Vector(i, j);
					    for(int k = 0; k < tmpPc.possAttacks.Length; k++)
					    {
						    Vector dest = Vector.add(src, tmpPc.possAttacks[k]);
						    if(dest.x>=0 && dest.x<=7 && dest.y>=0 && dest.y<=7 && isEmpty(dest)
							    && validateAttack(tmpPc, src, tmpPc.possAttacks[k]))
							    {
								    delete[count] = tiles[i][j]; //add the tile containing the coward piece in an array.
								    count++; //increment the number of cowards :D
								    break; //to avoid penalizing the same piece twice.
							    }
					    }
				    }
			    }
    		
		    //nullify the coward pieces !
		    for(int i = 0; i < count; i++)
			    delete[i].p = null;

            if (count != 0)
            {
                fruled = true; //a flag that should be true if at least one piece was penalized.
                attackAgain = null;
            }
	    }

        public bool isStuck()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    Piece tmpPc = tiles[i][j].p;
                    if (tmpPc != null && tmpPc.isHis(GamePlay.currPl))
                    {
                        Vector src = new Vector(i, j);
                        for (int k = 0; k < tmpPc.possMoves.Length; k++)
                        {
                            Vector dest = Vector.add(src, tmpPc.possMoves[k]);
                            if (dest.x >= 0 && dest.x <= 7 && dest.y >= 0 && dest.y <= 7
                                    && tiles[dest.x][dest.y].p == null
                                    && validateMove(tmpPc, src, tmpPc.possMoves[k]))
                                return false;
                        }
                        for (int k = 0; k < tmpPc.possAttacks.Length; k++)
                        {
                            Vector dest = Vector.add(src, tmpPc.possAttacks[k]);
                            if (dest.x >= 0 && dest.x <= 7 && dest.y >= 0 && dest.y <= 7
                                    && tiles[dest.x][dest.y].p == null
                                    && validateAttack(tmpPc, src, tmpPc.possAttacks[k]))
                                return false;
                        }
                    }
                }

            return true;
        }

        public void Update()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    tiles[i][j].Update();
        }

        public void Draw(Camera c)
        {
            ((BasicEffect)model.Meshes[0].Effects[0]).DirectionalLight0.Direction =
                Vector3.Negate(Vector3.Normalize(c.position));

            c.render(model, Matrix.Identity, aTrans);
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    tiles[i][j].Draw(c);
        }
    }
}
