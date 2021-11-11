# Periodization

This module defines how to

- initialize
- run
- finalize

for your

- applications
- infrastructures

and so on.

## Dependencies

Nothing.

## Architecture

| Word | Abstraction |
|:-|:-|
| Origination | Token to initialize. |
| Termination | Token to finalize. |
| Execution | Token to run. |
| Span | Lifetime from `Origination` to `Termination`. |
| Cycle | Lifecycle for `Execution` between `Origination` and `Termination` of `Span`. |

`Origination` initializes applications, infrastructures, and so on.
`Termination` finalizes applications, infrastructures, and so on.
`Execution` runs task for applications.
`Span` has `Origination` and `Termination`, and can can be converted from them.
`Cycle` has `Execution` and `Span`, and can be converted from them.

![Image not found.](./Resources/Periodization.jpg "Architecture of Periodization.")

## Implementation

Nothing except internal implementations for [Periodization](./Periodization.md).
