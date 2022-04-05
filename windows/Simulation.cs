namespace ReactionDiffusionSaver
{
    class Simulation
    {
        const int A = 1;
        const int B = 2;

        private double m_diffusionA = 1.0;
        private double m_diffusionB = 0.5;
        private double m_feedRate = 0.055;
        private double m_killRate = 0.062;

        private int m_width = 0;
        private int m_height = 0;

        private double[,] m_currentA;
        private double[,] m_currentB;

        private double[,] m_nextA;
        private double[,] m_nextB;

        public Simulation(int width, int height, double diffusionA = 1.0, double diffusionB = 0.5, double feedRate = 0.055, double killRate = 0.062)
        {
            m_width = width;
            m_height = height;

            m_diffusionA = diffusionA;
            m_diffusionB = diffusionB;
            m_feedRate = feedRate;
            m_killRate = killRate;

            m_currentA = new double[width, height];
            m_currentB = new double[width, height];

            m_nextA = new double[width, height];
            m_nextB = new double[width, height];

            Reset();
        }

        public void Reset(double a = 1, double b = 0)
        {
            for (int x = 0; x < m_width; x++)
            {
                for (int y = 0; y < m_height; y++)
                {
                    m_currentA[x, y] = m_nextA[x, y] = a;
                    m_currentB[x, y] = m_nextB[x, y] = b;
                }
            }
        }

        public double Get(int type, int x, int y)
        {
            return type == A ? m_currentA[x, y] : m_currentB[x, y];
        }

        public void Update()
        {
            double a, b;

            for (int x = 1; x < m_width - 1; x++)
            {
                for (int y = 1; y < m_height - 1; y++)
                {
                    a = m_currentA[x, y];
                    b = m_currentB[x, y];
                    m_nextA[x, y] = a + ((m_diffusionA * Laplace(A, x, y) * a) - (a * b * b) + (m_feedRate * (1 - a)));
                    m_nextB[x, y] = b + ((m_diffusionB * Laplace(B, x, y) * b) + (a * b * b) - ((m_killRate + m_feedRate) * b));

                    if (m_nextA[x, y] < 0)
                        m_nextA[x, y] = 0;
                    else if (m_nextA[x, y] > 1)
                        m_nextA[x, y] = 1;

                    if (m_nextB[x, y] < 0)
                        m_nextB[x, y] = 0;
                    else if (m_nextB[x, y] > 1)
                        m_nextB[x, y] = 1;
                }
            }

            for (int x = 1; x < m_width - 1; x++)
            {
                for (int y = 1; y < m_height - 1; y++)
                {
                    m_currentA[x, y] = m_nextA[x, y];
                    m_currentB[x, y] = m_nextB[x, y];
                }
            }
        }

        private double Laplace(int type, int x, int y)
        {
            double retval = (type == A ? m_currentA[x, y] : m_currentB[x, y]) * -1;

            // x + 1, y - 1
            retval += (type == A ? m_currentA[x + 1, y - 1] : m_currentB[x + 1, y - 1]) * 0.05;
            // x - 1, y + 1
            retval += (type == A ? m_currentA[x - 1, y + 1] : m_currentB[x - 1, y + 1]) * 0.05;
            // x - 1, y - 1
            retval += (type == A ? m_currentA[x - 1, y - 1] : m_currentB[x - 1, y - 1]) * 0.05;
            // x + 1, y + 1
            retval += (type == A ? m_currentA[x + 1, y + 1] : m_currentB[x + 1, y + 1]) * 0.05;
            // x - 1, y
            retval += (type == A ? m_currentA[x - 1, y] : m_currentB[x - 1, y]) * 0.2;
            // x, y - 1
            retval += (type == A ? m_currentA[x, y - 1] : m_currentB[x, y - 1]) * 0.2;
            // x + 1, y
            retval += (type == A ? m_currentA[x + 1, y] : m_currentB[x + 1, y]) * 0.2;
            // x, y + 1
            retval += (type == A ? m_currentA[x, y + 1] : m_currentB[x, y + 1]) * 0.2;

            return retval;
        }
    }
}
