SHELL = /bin/bash

ROOT_DIR = $(dir $(abspath $(lastword $(MAKEFILE_LIST))))
ifeq ($(OS),Windows_NT)
	SEP = \\
	RM = del /Q /F /S
	RRM = rmdir /S /Q
	MV = rename
	CMDSEP = &
	ROOT_DIR_CLEAN = $(subst /,\,$(ROOT_DIR))
else
	SEP = /
	RM = rm -f
	RRM = rm -f -r
	MV = mv -f
	ROOT_DIR_CLEAN = $(ROOT_DIR)
	ARCH?=$(shell uname -p)
endif

BUILD_DIR = build
PATHSEP = $(strip $(SEP))
SOURCES = $(wildcard src$(PATHSEP)*.cs)

all: $(BUILD_DIR) $(BUILD_DIR)/kuzzlesdk-csharp

$(BUILD_DIR):
ifeq ($(OS),Windows_NT)
	@if not exist $(subst /,\,$(BUILD_DIR)) mkdir $(subst /,\,$(BUILD_DIR))
else
	mkdir -p $@
endif

$(BUILD_DIR)/kuzzlesdk-csharp: $(SOURCES)
	@mcs -target:library -out:$@ $(SOURCES)

clean:
	$(RRM) $(BUILD_DIR)

.PHONY: all
