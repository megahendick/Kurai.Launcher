using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Animation;

namespace Kurai.Launcher.Utils;

public class DynamicDoubleAnimation : DoubleAnimation
{
  // Helper class to resolve bindings
  internal class BindingResolver<TValue> : DependencyObject
  {
    // Define property of type 'object' to make it nullable by default.
    // Introduces boxing in case 'TValue' is a 'ValueType'.
    public object ResolvedBindingValue
    {
      get => (object)GetValue(ResolvedBindingValueProperty);
      set => SetValue(ResolvedBindingValueProperty, value);
    }

    public static readonly DependencyProperty ResolvedBindingValueProperty = DependencyProperty.Register(
      "ResolvedBindingValue",
      typeof(object),
      typeof(BindingResolver<TValue>),
      new PropertyMetadata(default));

    // Returns 'false' when the binding couldn't be resolved
    public bool TryResolveBinding(BindingBase bindingBase, out TValue? resolvedBindingValue)
    {
      _ = BindingOperations.SetBinding(this, ResolvedBindingValueProperty, bindingBase);
      resolvedBindingValue = (TValue)this.ResolvedBindingValue;

      return this.ResolvedBindingValue is not null;
    }
  }

  public BindingBase? FromBinding { get; set; }
  public BindingBase? ToBinding { get; set; }
  private BindingResolver<double> BindingValueProvider { get; }

  public DynamicDoubleAnimation()
  {
    this.BindingValueProvider = new BindingResolver<double>();
  }

  // Because we extend ColorAnimation which is a Freezable, 
  // we must override Freezable.CreateInstanceCore too
  protected override Freezable CreateInstanceCore()
    => new DynamicDoubleAnimation();

  protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue, AnimationClock animationClock)
  {
    double fromDouble = this.From ?? defaultOriginValue;
    double toDouble = this.To ?? defaultDestinationValue;

    if (!this.From.HasValue
      && this.FromBinding is not null)
    {
      // Ignore the default value to give the defaultOriginValue 
      // parameter precedence in case the binding didn't resolve
      if (this.BindingValueProvider.TryResolveBinding(this.FromBinding, out double bindingValue))
      {
        fromDouble = bindingValue;
      }
    }

    if (!this.To.HasValue
      && this.ToBinding is not null)
    {
      // Ignore the default value to give the defaultOriginValue 
      // parameter precedence in case the binding didn't resolve
      if (this.BindingValueProvider.TryResolveBinding(this.ToBinding, out double bindingValue))
      {
        toDouble = bindingValue;
      }
    }

    return base.GetCurrentValueCore(fromDouble, toDouble, animationClock);
  }
}