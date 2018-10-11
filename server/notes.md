
Build Server Structure
======================

Nginx is installed and configured with a single site: `./nginx.site`. Although
this site is not encrypted, the entire site is reverse-proxied through the
main web server for [samuelharmer.com](https://samuelharmer.com) which has a
commercially signed SSL certificate. This is required so that the Webhook
(HTTP PUSH) from Gitlab can properly identify the server, and so that the
private token transferred in the Webhook is secure. This stops anyone in the
world from triggering builds.


Unity Editor
------------

Unity was downloaded from `unity-editor-2017.2.0f1+20170925.torrent` and was
installed to `/opt/Unity/Editor/`.

To build we using the following command.
```commandline
# /opt/Unity/Editor/Unity -quit -nographics -batchmode \
    -logFile stdout.log \
    -projectPath "$(pwd)" \
    -executeMethod BuildWebGL.DoBuild
```

To get the abbreviated hash of the currently checked-out commit we use
```commandline
# git show --format="%h" --no-patch
7e9442b
```


Git Repository Structure
------------------------

Within the `ankylosaurus` repository are a number of directories.
  - Artwork
  - Cyber Attack
  - server

The `Cyber Attack` directory is root of the Unity project. The size of the
repository would become unmanagable if we kept build artefacts or binaries in
version control so Git ignores were established early in the project to prevent
committing them. Here is a summary of our working Unity directory. Directories
marked with an `X` are not committed.

```
Cyber Attack
├── Assets
│   ├── Editor
│   ├── Fonts
│   ├── Materials
│   ├── Plugins
│   ├── Prefabs
│   ├── Scenes
│   └── Tiles
├X─ Builds
│   └── WebGL
│       └── CyberAttack
├X─ Library
│   └── …
├X─ obj
│   └── …
├── ProjectSettings
├X─ Temp
│   └── …
└── UnityPackageManager
```

The project, as seen within Unity, is the `Assets` directory.

TODO: describe the directory structure

Builds are compiled into `Builds/<platform>/CyberAttack`. A second directory
named `server` sits alongside the Unity directory and is responsible for the
WebGL release.

```
server/
├── built
│   ├── 2bcbd82
│   │   └── WebGL
│   │       └── CyberAttack
│   ├── 5347088
│   │   └── WebGL
│   │       └── CyberAttack
│   ├── 75ea6d0
│   │   └── WebGL
│   │       └── CyberAttack
│   └── 7e9442b
│       └── WebGL
│           ├── Build
│           └── TemplateData
├── current -> built/5347088/WebGL/CyberAttack/
└── nginx.site
```

At its root is the NGINX configuration. After each build is complete, the
directory is moved to the `server` directory (prefixed with a short version of
the commit hash). Once this is all complete, the symlink can be updated almost
atomically so there is never any downtime from a player's point of view.


Build Instructions
------------------

Log in to the Unity web server.

Update the repository.
```commandline
$ cd "/var/www/ankylosaurus/Cyber Attack/"
$ git pull origin master:master
```

Check that the `stable` branch has moved up to latest `master`.
```commandline
$ git log --oneline --abbrev-commit --all --graph --decorate
```

Perform the build.
```commandline
$ /usr/local/share/unity/Editor/Unity -quit -nographics -batchmode -logFile stdout.log -projectPath "$(pwd)" -executeMethod GameBuilder.BuildWebGL
```

Build progress can be observed by watching the log file on a separate terminal.
```commandline
$ tail -f "/var/www/ankylosaurus/Cyber Attack/stdout.log"
```

Once the build has completed successfully, update the server root.
```commandline
$ cd ../server/
$ export CYBERATTACKVERSION=$(git show --format="%h" --no-patch)
$ mkdir built/${CYBERATTACKVERSION}
$ mv "../Cyber Attack/Builds/WebGL/" built/${CYBERATTACKVERSION}/
$ rm current && ln -s built/${CYBERATTACKVERSION}/WebGL/CyberAttack/ current
```

If everything went smoothly, push the updated `stable` back to `origin` and confirm.
```commandline
$ git push
$ git log --oneline --abbrev-commit --all --graph --decorate
```
