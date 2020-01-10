/*
 * gadove_initialize.cpp
 *
 * Code generation for function 'gadove_initialize'
 *
 */

/* Include files */
#include "rt_nonfinite.h"
#include "gadove.h"
#include "gadove_initialize.h"
#include "eml_rand_mt19937ar_stateful.h"

/* Function Definitions */
void gadove_initialize()
{
  rt_InitInfAndNaN(8U);
  c_eml_rand_mt19937ar_stateful_i();
}

/* End of code generation (gadove_initialize.cpp) */
