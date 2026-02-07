# How the 3D Knots are Generated

This document explains the mathematical and rendering logic behind the Knot Visualizer application.

## 1. The Mathematical Core: Parametric Equations
At the heart of the visualization are **parametric equations**. These functions take a single variable, `t` (representing an angle from 0 to 2π), and calculate a 3D coordinate `(x, y, z)` in space.

### Torus Knots
A torus knot is defined by wrapping a curve around a torus (donut shape).
- **Formula**:
  ```csharp
  x = (R + r * cos(p * t)) * cos(q * t)
  y = (R + r * cos(p * t)) * sin(q * t)
  z = r * sin(p * t)
  ```
- **Parameters**:
  - `P` (Meridian): How many times the curve winds through the hole of the donut.
  - `Q` (Longitude): How many times the curve winds around the central axis of the donut.

### Lissajous Knots
These are created using harmonic motion (sine waves) on all three axes with different frequencies.
- **Formula**:
  ```csharp
  x = sin(nx * t)
  y = sin(ny * t + phase)
  z = sin(nz * t)
  ```
- **Frequencies (`nx`, `ny`, `nz`)**: Determine the complexity and "loops" of the knot in each dimension.

### Celtic Star Knots
This is a variation of a torus knot heavily modulated to create a woven, star-like appearance.
- **Logic**: We take a standard torus knot and modulate its radius `r` and height `z` using cosine functions to creat peaks and valleys, simulating an interlaced structure.

---

## 2. Generating the 3D Geometry (Tube Extrusion)
The equations above only give us a thin, 1D line. To render it as a 3D object, we must "extrude" a tube along this path.

### Step A: The Skeleton
We sample the parametric equation at many points (defined by the `Resolution` slider, e.g., 200 steps) to create a "skeleton" of the knot.

### Step B: The Frame (Frenet-Serret)
At each point on the skeleton, we need to know how to orient the tube's cross-section. We calculate a local coordinate system:
1.  **Tangent (T)**: The direction the curve is moving. Calculated by `Position(t + small_amount) - Position(t)`.
2.  **Binormal (B)**: A vector perpendicular to the Tangent and an arbitrary "up" vector.
3.  **Normal (N)**: The cross product of B and T, completing the orthogonal frame.

### Step C: The Skin
For every point on the skeleton, we generate a circle of vertices (the "ribs" of the tube) using the `Normal` and `Binormal` vectors.
```csharp
Vector3 offset = x * Normal + y * Binormal;
Vertex = SkeletonPoint + offset;
```

### Step D: Triangulation
Finally, we connect these rings of vertices with triangles. For every two adjacent rings, we create two triangles for each segment, stitching them into a solid mesh.

---

## 3. Rendering (Babylon.js)
The C# code sends this raw data (Vertex Positions + Indices) to JavaScript.
- **Engine**: Babylon.js renders the mesh using WebGL.
- **Shader**: We use `StandardMaterial` with a `DynamicTexture`.
- **Gradient**: A linear gradient (Green to Blue) is drawn onto a 2D canvas texture and wrapped around the 3D tube, giving it the vibrant, color-shifting look.
