using System;

class SplineInterpolation
{
    private double[] x;
    private double[] y;
    private double[] m;
    private double[] h;

    public SplineInterpolation(double[] x, double[] y)
    {
        this.x = x;
        this.y = y;
        this.h = new double[x.Length - 1];
        this.m = new double[x.Length];

        for (int i = 0; i < x.Length - 1; i++)
        {
            h[i] = x[i + 1] - x[i];
        }

        double[] alpha = new double[x.Length - 1];
        for (int i = 1; i < x.Length - 1; i++)
        {
            alpha[i] = 3 * (y[i + 1] - y[i]) / h[i] - 3 * (y[i] - y[i - 1]) / h[i - 1];
        }

        double[] l = new double[x.Length];
        double[] z = new double[x.Length];
        l[0] = 1;
        z[0] = 0;
        m[0] = m[m.Length - 1] = 0;

        for (int i = 1; i < x.Length - 1; i++)
        {
            l[i] = 2 * (x[i + 1] - x[i - 1]) - h[i - 1] * z[i - 1];
            z[i] = h[i] / l[i];
            m[i] = (alpha[i] - h[i - 1] * m[i - 1]) / l[i];
        }

        for (int j = x.Length - 2; j >= 0; j--)
        {
            m[j] = m[j] - z[j] * m[j + 1];
        }
    }

    public double Interpolate(double xi)
    {
        int index = Array.BinarySearch(x, xi);
        if (index < 0)
        {
            index = ~index - 1;
            if (index < 0)
            {
                index = 0;
            }
            if (index >= x.Length - 1)
            {
                index = x.Length - 2;
            }
        }

        double a = (x[index + 1] - xi) / h[index];
        double b = (xi - x[index]) / h[index];
        return a * y[index] + b * y[index + 1] + ((a * a * a - a) * m[index] + (b * b * b - b) * m[index + 1]) * h[index] * h[index] / 6;
    }
}

