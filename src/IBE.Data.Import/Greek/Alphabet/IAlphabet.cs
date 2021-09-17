namespace IBE.Data.Import.Greek.Alphabet {
    public interface IAlphabet {
        string Polish { get; }
        string Small { get; }
        string SmallAtTheEnd { get; }
        string Large { get; }

        string BreathingDashDash { get; }
        string BreathingDashDashSubscript { get; }
        string BreathingDashAcute { get; }
        string BreathingDashAcuteSubscript { get; }
        string BreathingDashGrave { get; }
        string BreathingDashGraveSubscript { get; }
        string BreathingDashCircumflex { get; }
        string BreathingDashCircumflexSubscript { get; }

        string BreathingSmoothDash { get; }
        string BreathingSmoothDashSubscript { get; }
        string BreathingSmoothAcute { get; }
        string BreathingSmoothAcuteSubscript { get; }
        string BreathingSmoothGrave { get; }
        string BreathingSmoothGraveSubscript { get; }
        string BreathingSmoothCircumflex { get; }
        string BreathingSmoothCircumflexSubscript { get; }

        string BreathingRoughDash { get; }
        string BreathingRoughDashSubscript { get; }
        string BreathingRoughAcute { get; }
        string BreathingRoughAcuteSubscript { get; }
        string BreathingRoughGrave { get; }
        string BreathingRoughGraveSubscript { get; }
        string BreathingRoughCircumflex { get; }
        string BreathingRoughCircumflexSubscript { get; }

        string BreathingDiaeresisDash { get; }
        string BreathingDiaeresisAcute { get; }
        string BreathingDiaeresisGrave { get; }
        string BreathingDiaeresisCircumflex { get; }

        string BreathingMacronDash { get; }
        string BreathingBreveDash { get; }

        string ReplaceWhen(string previous);
    }
}
