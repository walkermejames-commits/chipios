# ChipOS Integration TODOs

These are stub notes for the next integration phase.

## XR input

- `TODO`: replace `NRSDKInputSourceStub` with a real NRSDK/XREAL input adapter
- `TODO`: map hand rays, gaze focus, and confirm/select actions into `XRInputAction`
- `TODO`: decide how panel drag should behave with hand input versus gaze-only input

## Voice commands

- `TODO`: add a voice command adapter that routes spoken intents into `ChipOSManager`
- `TODO`: keep the adapter decoupled from the input router so voice can remain optional
- `TODO`: define a small first command set such as show, hide, focus, reset, and open task panel

## Live captions

- `TODO`: add a live caption panel type under `Assets/ChipOS/Scripts/UI/Panels`
- `TODO`: define caption data flow separately from notifications
- `TODO`: decide whether captions should pin near gaze or remain docked

## Camera and context

- `TODO`: add a camera/context adapter for scene understanding, object hints, or room mode updates
- `TODO`: feed environment-derived signals into `ContextState` without making `ContextState` platform-specific
- `TODO`: decide where image analysis or scene labels should surface in the UI

## AI assistant services

- `TODO`: add an assistant service adapter layer that can update `AssistantMessage` and related UI state
- `TODO`: keep assistant transport concerns out of `ChipOSManager`
- `TODO`: define what data can safely flow from device context into assistant requests
