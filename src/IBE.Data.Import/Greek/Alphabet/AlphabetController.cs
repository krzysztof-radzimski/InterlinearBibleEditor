using System;
using System.Collections.Generic;
using System.Text;

namespace IBE.Data.Import.Greek.Alphabet {
    public class AlphabetController {
        public Alfa Alfa { get; }
        public Beta Beta { get; }
        public Gamma Gamma { get; }
        public Delta Delta { get; }
        public Epsilon Epsilon { get; }
        public Zeta Zeta { get; }
        public Eta Eta { get; }
        public Theta Theta { get; }
        public Jota Jota { get; }
        public Kappa Kappa { get; }
        public Lambda Lambda { get; }
        public Mi Mi { get; }
        public Ni Ni { get; }
        public Ksi Ksi { get; }
        public Omikron Omikron { get; }
        public Pi Pi { get; }
        public Ro Ro { get; }
        public Sigma Sigma { get; }
        public Tau Tau { get; }
        public Ypsilon Ypsilon { get; }
        public Fi Fi { get; }
        public Chi Chi { get; }
        public Psi Psi { get; }
        public Omega Omega { get; }

        public AlphabetController() {
            Alfa = new Alfa();
            Beta = new Beta();
            Chi = new Chi();
            Delta = new Delta();
            Epsilon = new Epsilon();
            Eta = new Eta();
            Fi = new Fi();
            Gamma = new Gamma();
            Jota = new Jota();
            Kappa = new Kappa();
            Ksi = new Ksi();
            Lambda = new Lambda();
            Mi = new Mi();
            Ni = new Ni();
            Omega = new Omega();
            Omikron = new Omikron();
            Pi = new Pi();
            Psi = new Psi();
            Ro = new Ro();
            Sigma = new Sigma();
            Tau = new Tau();
            Theta = new Theta();
            Ypsilon = new Ypsilon();
            Zeta = new Zeta();
        }
    }
}
