#!/bin/bash

#当前所在路径(即zip文件所在路径)
CURRENT_PATH=$1
#本次调用脚本目标行为
ACTION=$2

DECOMPRESS="decompress"
MOVEFILE="movefile"

cd $CURRENT_PATH

if [ "$ACTION" = "$DECOMPRESS" ]
then
#zip文件名称
ZIP_FILE=$3
unzip $ZIP_FILE -x __MACOSX/*
fi

if [ "$ACTION" = "$MOVEFILE" ]
then
#Xcode项目根路径
TARGET_PATH=$3
#SDK名称(解压出的文件夹的名称)
SDK_NAME=$4
mv $SDK_NAME $TARGET_PATH
fi

