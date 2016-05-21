/* The size of these files were reduced. The following function fixes all references. */
var $customMSCore = JSIL.GetAssembly("mscorlib");
var $customSys = JSIL.GetAssembly("System");
var $customSysConf = JSIL.GetAssembly("System.Configuration");
var $customSysCore = JSIL.GetAssembly("System.Core");
var $customSysNum = JSIL.GetAssembly("System.Numerics");
var $customSysXml = JSIL.GetAssembly("System.Xml");
var $customSysSec = JSIL.GetAssembly("System.Security");

if (typeof (contentManifest) !== "object") { contentManifest = {}; };
contentManifest["Fusee.Tutorial.Web.contentproj"] = [
    ["Script",	"Fusee.Base.Core.Ext.js",	{  "sizeBytes": 1273 }],
    ["Script",	"Fusee.Base.Imp.Web.Ext.js",	{  "sizeBytes": 9244 }],
    ["Script",	"opentype.js",	{  "sizeBytes": 166330 }],
    ["Script",	"Fusee.Xene.Ext.js",	{  "sizeBytes": 1441 }],
    ["Script",	"Fusee.Xirkit.Ext.js",	{  "sizeBytes": 44215 }],
    ["Script",	"Fusee.Engine.Imp.Graphics.Web.Ext.js",	{  "sizeBytes": 105980 }],
    ["Script",	"SystemExternals.js",	{  "sizeBytes": 11976 }],
    ["Image",	"Assets/Leaves.jpg",	{  "sizeBytes": 35735 }],
    ["Image",	"Assets/litsphere.jpg",	{  "sizeBytes": 26951 }],
    ["File",	"Assets/PixelShader.frag",	{  "sizeBytes": 1051 }],
    ["File",	"Assets/PixelShaderGlobal.frag",	{  "sizeBytes": 128 }],
    ["Image",	"Assets/Styles/loading_rocket.png",	{  "sizeBytes": 10975 }],
    ["File",	"Assets/VertexShader.vert",	{  "sizeBytes": 823 }],
    ["File",	"Assets/VertexShaderGlobal.vert",	{  "sizeBytes": 404 }],
    ["File",	"Assets/WuggyLand.fus",	{  "sizeBytes": 4990046 }],
    ];