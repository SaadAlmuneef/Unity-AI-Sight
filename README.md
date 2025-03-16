# AI Sight System

an AI sight component for detecting gameobjects with other features

## Features
-  Field of View (FOV) detection
-  Detects visible targets within a configurable radius
- Aging Time: AI remembers lost targets for a set duration before forgetting them
- Configurable through ScriptableObject  
- Debug visualization with gizmos

 
## Usage
- Attach `AISightSenseComponent` to any AI character.
- Assign a `AISightConfig` ScriptableObject to configure
- Subscribe to `OnTargetSeen` and `OnTargetLost` events to trigger AI behavior.
