namespace Common.Exceptions
{
  public class ValidationException : Exception
  {
    private readonly string _model;

    public ValidationException(string message, string model) : base(message) {
      _model = model;
    }

    public override string ToString()
    {
      return $"ValidationException for {_model} model: {base.Message}";
    }
  }
}
