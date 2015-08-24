# Blueprint
Blueprint is a .net based static site generator using Markdown and Mustache to make content generating fast and easy.

## Usage
Blueprint is an executable that can be run through the command line with specific parameters, or just as a stand-alone executable alongside your website's files. 

`Blueprint.exe <action> <destination folder>` or `Blueprint.exe <action> <source folder> <destination folder>`

Blueprint currently only takes one action parameter called `manufacture`. Together with a destination folder to place the generated website in, are the only mandatory parameters needed to make blueprint do it's job.

## Configuration
Blueprint can be configured with a few extra settings. These are not mandatory but could be usefull if you for example want to include or exclude certain folders from your source directory. The configuration file can be found in `_config/config.json`.

- `debug` - run blueprint in debug mode, which includes seeing all output and progress of the executable, and making a site variable called `debug` available.
- `include` - By specifying a folder name, you can include it in your destination folder. By default all directories withouth an `_` in front of the name will be copied over to the destination directory

## Page Variables
Blueprint uses Nustache (Mustache for .net) as a simple html templating engine. Because of this it is possible to use dynamic content in pages and posts of your website.  
There are two  main variables available which currently hold all the available data. `site` contains all general information about the website, including lists of all pages and posts. `Page` contains page specific information like excerpts and/or categories.

- `Site.time` - The time of the blueprint build process
- `Site.Pages` - A list of all pages
- `Site.Posts` - A list of all posts
- `Site.RecentPosts` - A list of the top 10 most recent posts

- `Page.Title` - The title of the page
- `Page.Url` - The Url of the page
- `Page.Date` - The creation date of the page
- `Page.Excerpt` - The Excerpt of the page
