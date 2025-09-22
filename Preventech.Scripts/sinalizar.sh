#!/bin/env /bin/sh
PID_PREVENTECH=$(
    ps | # processos
    grep Preventech.Server.dll | # que contém Preventech.Server.dll
    awk '{ print $1 }' | # coluna pid
    head -n 1 # primeira linha, o grep vai aparecer no meio e retornar dois PIDs no final
)

kill -28 $PID_PREVENTECH