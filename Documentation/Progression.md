# Progression

This module defines how to

- initialize
- perform
- finalize

applications, infrastructures, and so on.

## Dependencies

Nothing.

## Architecture

![Image not found.](./Resources/Progression.jpg "Architecture of Progression.")

| Word | Abstraction |
|:-----------|:------------|
| Origination | Token to initialize. |
| Termination | Token to finalize. |
| Execution | Token to perform. |
| Procession | Token to process. |
| Abortion | Token to abort exception. |

`Origination` initializes applications, infrastructures, and so on.
`Termination` finalizes them.
`Execution` performs task.
`Procession` is a sequence to initialize, perform and finalize them.

`Abortion` can be bound to `Origination`, `Termination` and `Execution` to deal with exceptions thrown in them.

## Implementation

Nothing because this module only defines how to

- initialize
- perform
- finalize

applications, infrastructures, and so on.
