namespace IBE.ePubConverter.Common.Converters {
    public interface IConverter<TResult> {
        TResult Execute(string fileName, bool loadLicenseKey = true);
    }
}
