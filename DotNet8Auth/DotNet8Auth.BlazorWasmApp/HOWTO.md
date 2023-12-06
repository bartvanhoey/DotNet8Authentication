# How to

## How to add Blazor Components for Bootstrap Icons

Individual components for all of the [Bootstrap Icons](https://icons.getbootstrap.com/)

### How to Install

#### The Icon svg

Create a folder in your `wwwroot` folder: `wwwroot/images/bootstrap-icons`. Download [bootstrap-icons.svg](https://github.com/BrianLParker/BlazorBootstrapIcons/tree/master/BlazorBootstrapIcons/wwwroot/images/bootstrap-icons) to the folder.

#### Components

**Copy these three files to your solution:**

- [BaseIcon.razor](https://github.com/BrianLParker/BlazorBootstrapIcons/blob/master/BlazorBootstrapIcons/Views/Components/BaseIcon.razor)
- [BaseIcon.razor.cs](https://github.com/BrianLParker/BlazorBootstrapIcons/blob/master/BlazorBootstrapIcons/Views/Components/BaseIcon.razor.cs)
- [IconComponents.cs](https://github.com/BrianLParker/BlazorBootstrapIcons/blob/master/BlazorBootstrapIcons/Views/Components/IconComponents.cs)

You can adjust the namespaces if required. Include the the namespace in _Imports.razor

```html
@using BlazorBootstrapIcons.Views.Components.Icons
```

### Usage

```html
<EmojiSunglassesIcon />
```

By default the Icon uses the current text color in your document.

More info about getting started with [BlazorBootstrapIcons](https://github.com/BrianLParker/BlazorBootstrapIcons/tree/master/BlazorBootstrapIcons#blazor-components-for-bootstrap-icons)
