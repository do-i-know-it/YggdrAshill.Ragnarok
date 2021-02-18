# Periodization

Periodization defines how to initialize and finalize applications.

## Dependencies

Nothing.

## Architecture

![Image not found.](./Resources/Periodization.jpg "Architecture of Periodization.")

| Word | Abstraction |
|:-----------|:------------|
| Origination | Token to start. |
| Termination | Token to stop. |

`Origination` initializes applications, infrastructures, and so on.
`Origination` also creates `Termination` to finalize them.

## Implementation

Nothing because this module only defines how to initialize and finalize applications.
