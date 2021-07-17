# YggdrAshill.Ragnarok: an application lifecycle framework

Ragnarok defines how to

- initialize
- perform
- finalize
- abort

applications for mainly XR (VR/AR/MR).  
This framework is able to isolate definitions from implementations for specific platforms.

- ex) [Unity](https://unity.com/ja)
- ex) [Xamarin](https://docs.microsoft.com/ja-jp/xamarin/get-started/what-is-xamarin)
- ex) [Windows Presentation Foundation (WPF)](https://docs.microsoft.com/ja-jp/visualstudio/designers/getting-started-with-wpf?view=vs-2019)

## Dependencies

This framework depends on .NET Standard 2.0.

## Installation

Developers should

1. Go to [Release pages](https://github.com/do-i-know-it/YggdrAshill.Ragnarok/releases).
1. Download the latest version.

to use this framework.

## Usage

Please see [Sample application](./Samples/Program.cs).

## Architecture

This framework consists of core modules below.

- [Progression](./Documentation/Progression.md)

## Implementation

This framework includes [Implementation](./Documentation/Implementation.md) to provide implementations and extensions for above.

## Known issues

Please see [issues](https://github.com/do-i-know-it/YggdrAshill.Ragnarok/issues).

## Future works

Please see [GitHub Project for road map](https://github.com/do-i-know-it/YggdrAshill.Ragnarok/projects/1).

- Adds definitions for life cycle events.
  - ex) onInitialized
  - ex) onPaused
  - ex) onResumed
  - ex) onFinalized

## License

This framework is under the [MIT License](https://opensource.org/licenses/mit-license.php), see [LICENSE](./LICENSE.txt).

## Remarks

This framework is a part of YggdrAshill framework.  
Other frameworks will be produced soon for YggdrAshill.
