VERSION = 0.0.1

ROOT_DIR = $(dir $(abspath $(lastword $(MAKEFILE_LIST))))
ifeq ($(OS),Windows_NT)
	GOROOT ?= C:/Go
	GOCC ?= $(GOROOT)bin\go
	SEP = \\
	RM = del /Q /F /S
	RRM = rmdir /S /Q
	MV = rename
	CMDSEP = &
	ROOT_DIR_CLEAN = $(subst /,\,$(ROOT_DIR))
	LIB_PREFIX =
else
	GOROOT ?= /usr/local/go
	GOCC ?= $(GOROOT)/bin/go
	SEP = /
	RM = rm -f
	RRM = rm -f -r
	MV = mv -f
	CMDSEP = ;
	ROOT_DIR_CLEAN = $(ROOT_DIR)
	LIB_PREFIX = lib
endif

PATHSEP = $(strip $(SEP))
ROOTOUTDIR = $(ROOT_DIR)/build
SWIG = swig

CXXFLAGS = -g -fPIC -std=c++11 -MMD \
	-I.$(PATHSEP)include \
	-I.$(PATHSEP)sdk-cpp$(PATHSEP)include \
	-I.$(PATHSEP)sdk-cpp$(PATHSEP)src \
	-I.$(PATHSEP)sdk-cpp$(PATHSEP)sdk-c$(PATHSEP)build$(PATHSEP)kuzzle-c-sdk$(PATHSEP)include \
	-L.$(PATHSEP)sdk-cpp$(PATHSEP)sdk-c$(PATHSEP)build$(PATHSEP)kuzzle-c-sdk$(PATHSEP)lib

LDFLAGS = -lkuzzlesdk

SRCS = kcore_wrap.cxx
OBJS = $(SRCS:.cxx=.o)

# Ignore SWIG warning: 451-memory allocation
# http://www.swig.org/Doc1.3/Warnings.html#Warnings_nn12
IGNORED_SWIG_WARNING = -w451

all: csharp

kcore_wrap.o: kcore_wrap.cxx
	$(CXX) -c $< -o $@ $(CXXFLAGS) $(LDFLAGS)

makedir:
ifeq ($(OS),Windows_NT)
	@if not exist $(subst /,\,$(ROOTOUTDIR)) mkdir $(subst /,\,$(ROOTOUTDIR))
else
	mkdir -p $(ROOTOUTDIR)
endif

make_c_sdk:
	cd sdk-cpp/sdk-c && $(MAKE)

swig:
	$(SWIG) -Wextra $(IGNORED_SWIG_WARNING) -c++ -csharp -namespace Kuzzleio -dllimport kuzzle-wrapper-csharp.dll -outdir $(OUTDIR) -o $(SRCS) -I.$(PATHSEP)sdk-cpp$(PATHSEP)include -I.$(PATHSEP)sdk-cpp$(PATHSEP)sdk-c$(PATHSEP)include$(PATHSEP) -I.$(PATHSEP)sdk-cpp$(PATHSEP)src -I.$(PATHSEP)sdk-cpp$(PATHSEP)sdk-c$(PATHSEP)build$(PATHSEP) swig/core.i

make_lib:
	$(CXX) -shared kcore_wrap.o -o $(OUTDIR)$(SEP)$(LIB_PREFIX)kuzzle-wrapper-csharp.dll $(CXXFLAGS) $(LDFLAGS)

remove_so:
	rm -rf .$(PATHSEP)sdk-cpp$(PATHSEP)sdk-c$(PATHSEP)build$(PATHSEP)*.so*

csharp: OUTDIR=$(ROOTOUTDIR)
csharp: makedir make_c_sdk remove_so swig $(OBJS) make_lib
	mcs -target:library -out:$(OUTDIR)$(SEP)kuzzlesdk-$(VERSION).dll build/*.cs
	# rm -f build/*.cs

# Only works on linux
run_example:
	cp sdk-cpp/sdk-c/build/kuzzle-c-sdk/lib/libkuzzlesdk.so build/*.dll example
	cd example && mcs -r:kuzzlesdk-0.0.1.dll example.cs && LD_LIBRARY_PATH=. mono example.exe

clean:
	cd sdk-cpp && $(MAKE) clean
	rm -rf build

.PHONY: all clean swig csharp remove_so make_lib make_c_sdk makedir
