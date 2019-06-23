using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Relative_Orientation
{
    class Calculation
    {
        public static double[,] ROrient(double[,] LPoint, double[,] RPoint, double f)
        {
            double[] b = new double[3];
            double u = 0, v = 0, phiL = 0, omigaL = 0, kappaL = 0;
            double phiR = 0, omigaR = 0, kappaR = 0;

            double[,] r2 = new double[3, 3];
            double[,] RS = new double[6, 3];
            double[] N1 = new double[6];
            double[] N2 = new double[6];
            double[] Q = new double[6];
            Matrix A = new Matrix(6,5,"A");
            double[,] a = A.Detail;
            Matrix L = new Matrix(6, 1, "L");
            double[,] l = L.Detail;
            Matrix _A = new Matrix(5, 6, "_A");
            Matrix ATA = new Matrix(5, 5, "ATA");
            Matrix N_AA = new Matrix(5, 5, "N_AA");
            Matrix temp = new Matrix(5, 6, "temp");
            Matrix dX = new Matrix(5, 1, "dX");
            double[,] dx = new double[5, 1];

            f = f / 1000;
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 2; j++)
                {
                    LPoint[i, j] /= 1000;
                    RPoint[i, j] /= 1000;
                }

            double bx = LPoint[0,0] - RPoint[0,0];
            double by = 0, bz = 0;

            double[,] r1 = new double[3, 3];           
            r1[0, 0] = Math.Cos(phiL) * Math.Cos(kappaL) - Math.Sin(phiL) * Math.Sin(omigaL) * Math.Sin(kappaL);
            r1[0, 1] = -Math.Cos(phiL) * Math.Sin(kappaL) - Math.Sin(phiL) * Math.Sin(omigaL) * Math.Cos(kappaL);
            r1[0, 2] = -Math.Sin(phiL) * Math.Sin(omigaL);
            r1[1, 0] = Math.Cos(omigaL) * Math.Sin(kappaL);
            r1[1, 1] = Math.Cos(omigaL) * Math.Cos(kappaL);
            r1[1, 2] = -Math.Sin(omigaL);
            r1[2, 0] = Math.Sin(phiL) * Math.Cos(kappaL) + Math.Cos(phiL) * Math.Sin(omigaL) * Math.Sin(kappaL);
            r1[2, 1] = -Math.Sin(phiL) * Math.Sin(kappaL) + Math.Cos(phiL) * Math.Sin(omigaL) * Math.Cos(kappaL);
            r1[2, 2] = Math.Cos(phiL) * Math.Cos(omigaL);

            double[,] LS = new double[6,3];
            for (int i = 0; i < 6; i++)
            {
                LS[i,0] = r1[0, 0] * LPoint[i, 0] + r1[0, 1] * LPoint[i, 1] + r1[0, 2] * (-f);
                LS[i,1] = r1[1, 0] * LPoint[i, 0] + r1[1, 1] * LPoint[i, 1] + r1[1, 2] * (-f);
                LS[i,2] = r1[2, 0] * LPoint[i, 0] + r1[2, 1] * LPoint[i, 1] + r1[2, 2] * (-f);
            }

            do{
                r2[0, 0] = Math.Cos(phiR) * Math.Cos(kappaR) - Math.Sin(phiR) * Math.Sin(omigaR) * Math.Sin(kappaR);
                r2[0, 1] = -Math.Cos(phiR) * Math.Sin(kappaR) - Math.Sin(phiR) * Math.Sin(omigaR) * Math.Cos(kappaR);
                r2[0, 2] = -Math.Sin(phiR) * Math.Sin(omigaR);
                r2[1, 0] = Math.Cos(omigaR) * Math.Sin(kappaR);
                r2[1, 1] = Math.Cos(omigaR) * Math.Cos(kappaR);
                r2[1, 2] = -Math.Sin(omigaR);
                r2[2, 0] = Math.Sin(phiR) * Math.Cos(kappaR) + Math.Cos(phiR) * Math.Sin(omigaR) * Math.Sin(kappaR);
                r2[2, 1] = -Math.Sin(phiR) * Math.Sin(kappaR) + Math.Cos(phiR) * Math.Sin(omigaR) * Math.Cos(kappaR);
                r2[2, 2] = Math.Cos(phiR) * Math.Cos(omigaR);

                by = bx * u;
                bz = bx * v;

                for (int i = 0; i < 6; i++)
                {
                    RS[i,0] = r2[0, 0] * RPoint[i, 0] + r2[0, 1] * RPoint[i, 1] + r2[0, 2] * (-f);
                    RS[i,1] = r2[1, 0] * RPoint[i, 0] + r2[1, 1] * RPoint[i, 1] + r2[1, 2] * (-f);
                    RS[i,2] = r2[2, 0] * RPoint[i, 0] + r2[2, 1] * RPoint[i, 1] + r2[2, 2] * (-f);
                }

                for (int i = 0; i < 6; i++)
                {
                    N1[i] = (bx * RS[i,2] - bz * RS[i,0]) / (LS[i,0] * RS[i,2] - RS[i,0] * LS[i,2]);
                    N2[i] = (bx * LS[i,2] - bz * LS[i,0]) / (LS[i,0] * RS[i,2] - RS[i,0] * LS[i,2]);
                    Q[i] = N1[i] * LS[i,1] - N2[i] * RS[i,1] - by;
                }

                for (int i = 0; i < 6; i++)
                {
                    a[i, 0] = bx;
                    a[i, 1] = (-RS[i,1]) * bx / RS[i,2];
                    a[i, 2] = (-RS[i,1]) * RS[i,0] * N2[i] / RS[i,2];
                    a[i, 3] = (-N2[i]) * (RS[i,2] + RS[i,1] * RS[i,1] / RS[i,2]);
                    a[i, 4] = RS[i,0] * N2[i];
                }
            
                for (int i = 0; i < 6; i++)
                {
                    l[i, 0] = Q[i];
                }

                _A = MatrixOperator.MatrixTrans(A);
                ATA = MatrixOperator.MatrixMulti(_A, A);
                N_AA = MatrixOperator.MatrixInvByCom(ATA);        
                temp = MatrixOperator.MatrixMulti(N_AA, _A);
                dX = MatrixOperator.MatrixMulti(temp, L);
                dx = dX.Detail;
                u += dx[0,0];
                v += dx[1,0];
                phiR += dx[2,0];
                omigaR += dx[3,0];
                kappaR += dx[4,0];
            } while (Math.Abs(dx[0, 0]) >= 0.00003 || Math.Abs(dx[1, 0]) >= 0.00003 || Math.Abs(dx[2, 0]) >= 0.00003 || Math.Abs(dx[3, 0]) >= 0.00003 || Math.Abs(dx[4, 0]) >= 0.00003);
            dx[0, 0] = u;
            dx[1, 0] = v;
            dx[2, 0] = phiR;
            dx[3, 0] = omigaR;
            dx[4, 0] = kappaR;
            return dx;                                                                                                      
        }
    }
}
