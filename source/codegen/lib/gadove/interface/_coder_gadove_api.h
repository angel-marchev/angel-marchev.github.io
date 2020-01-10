/*
 * _coder_gadove_api.h
 *
 * Code generation for function '_coder_gadove_api'
 *
 */

#ifndef _CODER_GADOVE_API_H
#define _CODER_GADOVE_API_H

/* Include files */
#include "tmwtypes.h"
#include "mex.h"
#include "emlrt.h"
#include <stddef.h>
#include <stdlib.h>
#include "_coder_gadove_api.h"

/* Type Definitions */
#ifndef struct_emxArray_real_T
#define struct_emxArray_real_T

struct emxArray_real_T
{
  real_T *data;
  int32_T *size;
  int32_T allocatedSize;
  int32_T numDimensions;
  boolean_T canFreeData;
};

#endif                                 /*struct_emxArray_real_T*/

#ifndef typedef_emxArray_real_T
#define typedef_emxArray_real_T

typedef struct emxArray_real_T emxArray_real_T;

#endif                                 /*typedef_emxArray_real_T*/

/* Variable Declarations */
extern emlrtCTX emlrtRootTLSGlobal;
extern emlrtContext emlrtContextGlobal;

/* Function Declarations */
extern void gadove(real_T x, real_T y, real_T z, emxArray_real_T *rec, real_T
                   best[3], real_T *iter);
extern void gadove_api(const mxArray * const prhs[3], int32_T nlhs, const
  mxArray *plhs[3]);
extern void gadove_atexit(void);
extern void gadove_initialize(void);
extern void gadove_terminate(void);
extern void gadove_xil_terminate(void);

#endif

/* End of code generation (_coder_gadove_api.h) */
