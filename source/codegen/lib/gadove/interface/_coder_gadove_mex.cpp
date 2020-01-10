/*
 * _coder_gadove_mex.cpp
 *
 * Code generation for function '_coder_gadove_mex'
 *
 */

/* Include files */
#include "_coder_gadove_api.h"
#include "_coder_gadove_mex.h"

/* Function Declarations */
static void gadove_mexFunction(int32_T nlhs, mxArray *plhs[3], int32_T nrhs,
  const mxArray *prhs[3]);

/* Function Definitions */
static void gadove_mexFunction(int32_T nlhs, mxArray *plhs[3], int32_T nrhs,
  const mxArray *prhs[3])
{
  const mxArray *outputs[3];
  int32_T b_nlhs;
  emlrtStack st = { NULL,              /* site */
    NULL,                              /* tls */
    NULL                               /* prev */
  };

  st.tls = emlrtRootTLSGlobal;

  /* Check for proper number of arguments. */
  if (nrhs != 3) {
    emlrtErrMsgIdAndTxt(&st, "EMLRT:runTime:WrongNumberOfInputs", 5, 12, 3, 4, 6,
                        "gadove");
  }

  if (nlhs > 3) {
    emlrtErrMsgIdAndTxt(&st, "EMLRT:runTime:TooManyOutputArguments", 3, 4, 6,
                        "gadove");
  }

  /* Call the function. */
  gadove_api(prhs, nlhs, outputs);

  /* Copy over outputs to the caller. */
  if (nlhs < 1) {
    b_nlhs = 1;
  } else {
    b_nlhs = nlhs;
  }

  emlrtReturnArrays(b_nlhs, plhs, outputs);
}

void mexFunction(int32_T nlhs, mxArray *plhs[], int32_T nrhs, const mxArray
                 *prhs[])
{
  mexAtExit(gadove_atexit);

  /* Module initialization. */
  gadove_initialize();

  /* Dispatch the entry-point. */
  gadove_mexFunction(nlhs, plhs, nrhs, prhs);

  /* Module termination. */
  gadove_terminate();
}

emlrtCTX mexFunctionCreateRootTLS(void)
{
  emlrtCreateRootTLS(&emlrtRootTLSGlobal, &emlrtContextGlobal, NULL, 1);
  return emlrtRootTLSGlobal;
}

/* End of code generation (_coder_gadove_mex.cpp) */
