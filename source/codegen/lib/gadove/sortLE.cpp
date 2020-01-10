/*
 * sortLE.cpp
 *
 * Code generation for function 'sortLE'
 *
 */

/* Include files */
#include "rt_nonfinite.h"
#include "gadove.h"
#include "sortLE.h"

/* Function Definitions */
boolean_T sortLE(const double v[45], int idx1, int idx2)
{
  boolean_T p;
  int k;
  boolean_T exitg1;
  double d0;
  double d1;
  p = true;
  k = 0;
  exitg1 = false;
  while ((!exitg1) && (k < 3)) {
    d0 = v[(idx1 + 15 * k) - 1];
    d1 = v[(idx2 + 15 * k) - 1];
    if ((d0 == d1) || (rtIsNaN(d0) && rtIsNaN(d1))) {
      k++;
    } else {
      if ((d0 >= d1) || rtIsNaN(d0)) {
      } else {
        p = false;
      }

      exitg1 = true;
    }
  }

  return p;
}

/* End of code generation (sortLE.cpp) */
