let engine;
let scene;
let camera;
let knotMesh;

window.initBabylonScene = function () {
    const canvas = document.getElementById('renderCanvas');
    engine = new BABYLON.Engine(canvas, true, { preserveDrawingBuffer: true, stencil: true });

    scene = new BABYLON.Scene(engine);
    scene = new BABYLON.Scene(engine);
    scene.clearColor = new BABYLON.Color4(0, 0, 0, 0); // Transparent to show CSS gradient

    // Create a slick gradient background using a Layer
    var background = new BABYLON.Layer("back", null, scene);
    background.isBackground = true;
    background.texture = new BABYLON.DynamicTexture("dynamicBackground", 512, scene, true);
    var ctx = background.texture.getContext();
    var grad = ctx.createLinearGradient(0, 0, 512, 512);
    grad.addColorStop(0, "#ffffff");
    grad.addColorStop(1, "#e6e9f0"); // Subtle white-to-gray
    ctx.fillStyle = grad;
    ctx.fillRect(0, 0, 512, 512);
    background.texture.update();

    // Camera setup
    camera = new BABYLON.ArcRotateCamera(
        "camera",
        Math.PI / 4,
        Math.PI / 3,
        15,
        BABYLON.Vector3.Zero(),
        scene
    );
    camera.attachControl(canvas, true);
    camera.lowerRadiusLimit = 5;
    camera.upperRadiusLimit = 50;
    camera.wheelPrecision = 50;
    camera.panningSensibility = 0;

    // Lighting
    const hemisphericLight = new BABYLON.HemisphericLight(
        "hemiLight",
        new BABYLON.Vector3(0, 1, 0),
        scene
    );
    hemisphericLight.intensity = 0.8;
    hemisphericLight.diffuse = new BABYLON.Color3(1, 1, 1);
    hemisphericLight.groundColor = new BABYLON.Color3(0.8, 0.8, 0.8);

    const directionalLight1 = new BABYLON.DirectionalLight(
        "dirLight1",
        new BABYLON.Vector3(-1, -2, -1),
        scene
    );
    directionalLight1.intensity = 0.7;
    directionalLight1.diffuse = new BABYLON.Color3(1, 0.95, 0.9);

    const directionalLight2 = new BABYLON.DirectionalLight(
        "dirLight2",
        new BABYLON.Vector3(1, -1, 1),
        scene
    );
    directionalLight2.intensity = 0.4;
    directionalLight2.diffuse = new BABYLON.Color3(0.5, 0.7, 1);

    // Render loop
    engine.runRenderLoop(() => {
        scene.render();
    });

    // Handle resize
    window.addEventListener('resize', () => {
        engine.resize();
    });
};

window.updateKnotMesh = function (vertices, indices) {
    // Remove old mesh
    if (knotMesh) {
        knotMesh.dispose();
    }

    // Create custom mesh
    knotMesh = new BABYLON.Mesh("knot", scene);

    const positions = new Float32Array(vertices);
    const indicesArray = new Uint32Array(indices);

    // Compute normals
    const normals = [];
    BABYLON.VertexData.ComputeNormals(positions, indicesArray, normals);

    const vertexData = new BABYLON.VertexData();
    vertexData.positions = positions;
    vertexData.indices = indicesArray;
    vertexData.normals = normals;

    vertexData.applyToMesh(knotMesh);

    // Create gradient material
    const material = new BABYLON.StandardMaterial("knotMat", scene);

    // Create a gradient texture
    const dynamicTexture = new BABYLON.DynamicTexture("gradientTexture", 512, scene, false);
    const ctx = dynamicTexture.getContext();

    // Create gradient
    // Create Green-Blue gradient
    const gradient = ctx.createLinearGradient(0, 0, 512, 512);
    gradient.addColorStop(0, '#00C853');   // Vibrant Green
    gradient.addColorStop(0.5, '#00B0FF'); // Light Blue
    gradient.addColorStop(1, '#2962FF');   // Deep Blue

    ctx.fillStyle = gradient;
    ctx.fillRect(0, 0, 512, 512);
    dynamicTexture.update();

    material.diffuseTexture = dynamicTexture;
    material.specularColor = new BABYLON.Color3(0.5, 0.5, 0.5);
    material.specularPower = 64;
    material.ambientColor = new BABYLON.Color3(0.2, 0.2, 0.3);

    knotMesh.material = material;

    // Add subtle animation
    let time = 0;
    scene.onBeforeRenderObservable.clear();
    scene.onBeforeRenderObservable.add(() => {
        time += 0.01;
        knotMesh.rotation.y = time * 0.2;
    });
};

window.resetCamera = function () {
    if (camera) {
        camera.alpha = Math.PI / 4;
        camera.beta = Math.PI / 3;
        camera.radius = 15;
        camera.target = BABYLON.Vector3.Zero();
    }
};
