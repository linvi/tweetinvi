#! /usr/local/bin/python3

import argparse
import subprocess
import sys
import os
import pathlib

parser = argparse.ArgumentParser()
parser.add_argument('--install', action='store_true')
parser.add_argument('--uninstall', action='store_true')
parser.add_argument('--add', action='store_true')
parser.add_argument('--rm', action='store_true')
args = parser.parse_args()

scriptDirectoryPath = sys.path[0]

if args.install:
    print('bootstrap in progress...')
    os.popen('nuget sources add -Name tweetinvi -Source $(pwd)').read()
    os.popen('nuget sources add -Name tweetinvi -ConfigFile ~/.nuget/NuGet/NuGet.Config -Source ' + scriptDirectoryPath).read()
    os.popen('cp ./tweetinvi-builder.py /usr/local/bin').read()
    os.popen('cp ./tweetinvi-deploy-locally.py /usr/local/bin').read()
    rootPath = str(pathlib.Path.home())

    if len(os.popen('cat ' + rootPath + '/.zshrc | grep TWEETINVI_BUILDER_PATH').read()) == 0:
        zshrc = open(rootPath + "/.zshrc", "a+")
        zshrc.write('\nexport TWEETINVI_BUILDER_PATH=' + scriptDirectoryPath)
        zshrc.close()

    if len(os.popen('cat ' + rootPath + '/.bash_profile | grep TWEETINVI_BUILDER_PATH').read()) == 0:
        bashProfile = open(rootPath + "/.bash_profile", "a+")
        bashProfile.write('\nexport TWEETINVI_BUILDER_PATH=' + scriptDirectoryPath)
        bashProfile.close()
    
    sys.exit(0)

if args.uninstall:
    print('uninstalling in progress...')
    os.popen('nuget sources remove -Name tweetinvi').read()
    os.popen('nuget sources remove -Name tweetinvi -ConfigFile ~/.nuget/NuGet/NuGet.Config').read()
    os.popen('rm /usr/local/bin/tweetinvi-deploy-locally.py').read()
    os.popen('rm /usr/local/bin/tweetinvi-builder.py').read()
    sys.exit(0)

tweetinviNugetSourceInstalled = len(os.popen('nuget sources | grep tweetinvi').readlines()) == 0

if tweetinviNugetSourceInstalled:
    print('You need to bootstrap your machine to run the build')
    print('Please run ./tweetinvi-deploy-locally.py --install')
    sys.exit(1)

version = os.popen('$TWEETINVI_BUILDER_PATH/tweetinvi-builder.py --build-version').read()
print('tweetinvi-builder.py currently targets version ' + version)

if args.add:
    subprocess.call('dotnet add package tweetinviapi -v ' + version, shell=True)

if args.rm:
    subprocess.call('dotnet remove package tweetinviapi', shell=True)