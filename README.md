# YggdrAshill.Ragnarok: an application lifecycle framework

Ragnarok defines how to

- Initialize
- Execute
- Abort
- Finalize

applications for mainly XR (VR/AR/MR).  
This framework is able to isolate definitions from implementations for specific platforms.

- ex) [Unity](https://unity.com/ja)
- ex) [Xamarin](https://docs.microsoft.com/ja-jp/xamarin/get-started/what-is-xamarin)
- ex) [Windows Presentation Foundation (WPF)](https://docs.microsoft.com/ja-jp/visualstudio/designers/getting-started-with-wpf?view=vs-2019)

<!-- ## Specifications

Now writing...

### Normal scenarios

Now writing...

### Abnormal scenarios

Now writing... -->

## Dependencies

Ragnarok depends on .NET Standard 2.0.

## Installation

In future, we will deploy dlls built to this repository, but now you should

1. Clone this repository.
1. Open cloned directory with Visual Studio.
1. Build for any CPU as Debug or Release.
1. Include built dlls to your project.

<!-- ## Usage

Now writing samples for this framework. -->

## Architecture

Ragnarok consists of core modules below.

- [Periodization](./Documentation/Periodization.md)
- [Progression](./Documentation/Progression.md)
- [Violation](./Documentation/Violation.md)

## Implementation

Ragnarok includes [Implementation](./Documentation/Implementation.md) to provide implementations and extensions for above.

## Known issues

Nothing now.

## Future works

### ~ Version 1.0.0

- Writes document comments in codes completely.
  - [Periodization](./Documentation/Periodization.md)
  - [Progression](./Documentation/Progression.md)
  - [Violation](./Documentation/Violation.md)
  - [Implementation](./Documentation/Implementation.md)
- Writes test codes for specification completely.
  - [Violation](./Documentation/Violation.md)
  - [Implementation](./Documentation/Implementation.md)
- Adds templates.
  - pull request
  - issue
  - contributing guidelines
- Adds samples.

### Version 1.0.0 ~

- Adds definitions for life cycle events.
  - ex) onInitialized
  - ex) onPaused
  - ex) onResumed
  - ex) onFinalized

## License

Ragnarok is under the [MIT License](https://opensource.org/licenses/mit-license.php), see [LICENSE](./LICENSE.txt).

## Remarks

Ragnarok is a part of YggdrAshill framework.  
Other frameworks will be produced soon for YggdrAshill.
