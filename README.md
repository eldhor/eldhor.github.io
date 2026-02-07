# Mathematical Knot Visualizer

An interactive 3D web application built with **Blazor WebAssembly** that visualizes mathematical knots based on knot theory. Features real-time parametric equation manipulation, smooth 3D rendering, and a modern UI.

## 🌟 Features

- **3D Knot Visualization**: Real-time rendering of mathematical knots using Babylon.js
- **Multiple Knot Types**:
  - Trefoil Knot (3,2)
  - Figure-Eight Knot
  - Cinquefoil Knot (5,2)
  - Custom Torus Knots with adjustable P and Q parameters
- **Interactive Controls**: 
  - Adjustable tube radius
  - Scalable knot size
  - Resolution control
  - Camera reset
- **Modern UI**: Built with MudBlazor for a sleek, professional interface
- **Parametric Mathematics**: All knots generated using pure mathematical equations

## 🚀 Quick Start

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- A modern web browser (Chrome, Firefox, Edge, Safari)

### Running Locally

1. **Clone or download this project**

2. **Navigate to the project directory**
   ```bash
   cd KnotVisualizer
   ```

3. **Restore dependencies**
   ```bash
   dotnet restore
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Open your browser** to `https://localhost:5001` (or the URL shown in the console)

## 📦 Deployment to GitHub Pages

This app is designed for static hosting and works perfectly with GitHub Pages.

### Step 1: Publish the Application

```bash
dotnet publish -c Release -o publish
```

### Step 2: Prepare for GitHub Pages

1. Add a `.nojekyll` file to the `publish/wwwroot` folder:
   ```bash
   touch publish/wwwroot/.nojekyll
   ```

2. Update the `<base href="/">` in `wwwroot/index.html` to match your GitHub Pages URL:
   ```html
   <base href="/your-repo-name/" />
   ```

### Step 3: Deploy

1. Create a new GitHub repository
2. Push your code to the repository
3. Copy the contents of `publish/wwwroot` to a `gh-pages` branch:
   ```bash
   git checkout --orphan gh-pages
   git rm -rf .
   cp -r publish/wwwroot/* .
   git add .
   git commit -m "Deploy to GitHub Pages"
   git push origin gh-pages
   ```
4. Enable GitHub Pages in your repository settings, selecting the `gh-pages` branch

Your app will be live at `https://yourusername.github.io/your-repo-name/`

## 🎮 Usage

1. **Select a Knot Type** from the dropdown menu
2. **Adjust Parameters**:
   - For Torus Knots: Set P and Q values to create different knot configurations
   - Tube Radius: Change the thickness of the knot
   - Scale: Resize the entire knot
   - Resolution: Increase for smoother curves (may affect performance)
3. **Click "Update Knot"** to regenerate the 3D model
4. **Interact with the 3D View**:
   - Click and drag to rotate
   - Scroll to zoom in/out
   - Use "Reset Camera" to return to the default view

## 🧮 Mathematics Behind the Knots

### Trefoil Knot (3,2)
```
x(t) = sin(t) + 2sin(2t)
y(t) = cos(t) - 2cos(2t)
z(t) = -sin(3t)
```

### Figure-Eight Knot
```
x(t) = (2 + cos(2t))cos(3t)
y(t) = (2 + cos(2t))sin(3t)
z(t) = sin(4t)
```

### Torus Knot (p,q)
```
x(t) = (R + r·cos(pt))cos(qt)
y(t) = (R + r·cos(pt))sin(qt)
z(t) = r·sin(pt)
```

## 🛠️ Technology Stack

- **Framework**: Blazor WebAssembly (.NET 8)
- **UI Library**: MudBlazor 6.19.1
- **3D Engine**: Babylon.js (via JavaScript Interop)
- **Language**: C# with parametric equation modeling
- **Styling**: Custom CSS with modern gradients and backdrop blur

## 📁 Project Structure

```
KnotVisualizer/
├── Models/
│   └── KnotGeometry.cs       # Mathematical knot equations and mesh generation
├── Pages/
│   └── Index.razor            # Main page with 3D canvas and controls
├── Shared/
│   └── MainLayout.razor       # App layout and theme
├── wwwroot/
│   ├── css/
│   │   └── app.css            # Custom styling
│   ├── js/
│   │   └── babylon-scene.js   # Babylon.js scene setup
│   └── index.html             # HTML template
├── Program.cs                 # App entry point
└── KnotVisualizer.csproj     # Project file
```

## 🎨 Customization

### Adding New Knot Types

1. Add a new enum value to `KnotType` in `Models/KnotGeometry.cs`
2. Implement the parametric equation in `GetKnotPosition()`
3. Add the knot to the dropdown in `Pages/Index.razor`

### Changing Colors

Modify the gradient in `babylon-scene.js`:
```javascript
gradient.addColorStop(0, '#YourColor1');
gradient.addColorStop(0.5, '#YourColor2');
gradient.addColorStop(1, '#YourColor3');
```

### Adjusting Theme

Edit the `_theme` object in `Shared/MainLayout.razor`

## 📝 License

This project is open source and available under the MIT License.

## 🤝 Contributing

Contributions, issues, and feature requests are welcome! Feel free to check the issues page.

## 🌐 Browser Compatibility

- Chrome/Edge: ✅ Fully supported
- Firefox: ✅ Fully supported
- Safari: ✅ Fully supported
- Mobile browsers: ✅ Supported (touch controls work for rotation)

## ⚡ Performance Tips

- Lower the resolution (50-100) for smoother interaction on less powerful devices
- Reduce tubular segments for better performance
- The app automatically runs at 60 FPS when possible

## 📚 Resources

- [Knot Theory - Wikipedia](https://en.wikipedia.org/wiki/Knot_theory)
- [Blazor Documentation](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
- [MudBlazor](https://mudblazor.com/)
- [Babylon.js](https://www.babylonjs.com/)

---

**Built with ❤️ using Blazor WebAssembly and mathematical knot theory**
